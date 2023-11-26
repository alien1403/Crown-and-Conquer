using System;
using System.Collections.Generic;
using UnityEngine;
using static ChunkPropertiesScriptableObject;

public class MapController : MonoBehaviour, IDataPersistence
{
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerMovement playerMovement;
    public GameObject currentChunk;
    [Header("Chunks")]
    public List<GameObject> baseChunks;
    public List<GameObject> forestChunks;
    public List<GameObject> marginChunks;
    [Header("Background")]
    public GameObject CameraTarget;
    public List<BackgroundLayersSpeed> baseBackgroundLayers;
    public List<BackgroundLayersSpeed> forestBackgroundLayers;
    private List<BackgroundLayersSpeed>[] backgroundLayers = new List<BackgroundLayersSpeed>[Enum.GetValues(typeof(ChunkType)).Length];

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestSpawnedChunk;
    public float maxOpDist;
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDuration;
    private ChunkType currentChunkType { get; set; }
    private ChunkType previousChunkType { get; set; }

    [Header("World Generation")]
    public GameObject parentObject;
    public int MinLeftForestChunks;
    public int MinRightForestChunks;
    public int MinLeftBaseChunks;
    public int MinRightBaseChunks;
    public int MinLeftIntermediaryChunks;
    public int MinRightIntermediaryChunks;
    public int MaxLeftForestChunks;
    public int MaxRightForestChunks;
    public int MaxLeftBaseChunks;
    public int MaxRightBaseChunks;
    public int MaxLeftIntermediaryChunks;
    public int MaxRightIntermediaryChunks;

    private float leftMostX;
    private float rightMostX;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        backgroundLayers[(int)ChunkType.Base] = baseBackgroundLayers;
        backgroundLayers[(int)ChunkType.Forest] = forestBackgroundLayers;
        foreach (int enumValue in Enum.GetValues(typeof(ChunkType)))
        {
            foreach (BackgroundLayersSpeed layer in backgroundLayers[enumValue])
            {
                GameObject layerReference = layer.GameObject;
                Parallax parallax = layerReference.GetComponent<Parallax>();
                parallax.parallaxEffect = layer.LayerSpeed;
                parallax.camera = CameraTarget;
            }
        }
        
    }

    void Update()
    {
        ChunkOptimizer();
    }
    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;
        if (optimizerCooldown > 0f)
        {
            return;
        }
        optimizerCooldown = optimizerCooldownDuration;
        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
    public void UpdateCurrentChunk(ChunkType current)
    {
        previousChunkType = currentChunkType;
        currentChunkType = current;
    }
    public void CheckBackgroundChunk()
    {
        if (currentChunkType != previousChunkType)
        {
            int previousSize = backgroundLayers[(int)previousChunkType].Count;
            int currentSize = backgroundLayers[(int)currentChunkType].Count;
            int counter = 0;
            float delay = 0, delayIncrease = 0.25f;
            for (; counter < previousSize && counter < currentSize; counter++, delay += delayIncrease)
            {
                Fade fadeOut = backgroundLayers[(int)previousChunkType][counter].GameObject.GetComponent<Fade>();
                Fade fadeIn = backgroundLayers[(int)currentChunkType][counter].GameObject.GetComponent<Fade>();
                StartCoroutine(fadeOut.StartFadeWithDelay(fadeOut.DoFadeOut, delay));
                StartCoroutine(fadeIn.StartFadeWithDelay(fadeIn.DoFadeIn, delay));
            }

            for (; counter < previousSize; counter++, delay += delayIncrease)
            {
                Fade fadeOut = backgroundLayers[(int)previousChunkType][counter].GameObject.GetComponent<Fade>();
                StartCoroutine(fadeOut.StartFadeWithDelay(fadeOut.DoFadeOut, delay));
            }
            for (; counter < currentSize; counter++, delay += delayIncrease)
            {
                Fade fadeIn = backgroundLayers[(int)currentChunkType][counter].GameObject.GetComponent<Fade>();
                StartCoroutine(fadeIn.StartFadeWithDelay(fadeIn.DoFadeIn, delay));
            }
        }
    }
    public void GenerateWorld()
    {
        System.Random random = new System.Random();
        int LeftBaseChunks = random.Next(MinLeftBaseChunks, MaxLeftBaseChunks + 1);
        int FirstLeftForestChunks = random.Next(MinLeftForestChunks, MaxLeftForestChunks + 1);
        int SecondLeftForestChunks = random.Next(MinLeftForestChunks, MaxLeftForestChunks + 1);
        int LeftIntermediaryChunks = random.Next(MinLeftIntermediaryChunks, MaxLeftIntermediaryChunks + 1);
        int RightBaseChunks = random.Next(MinRightBaseChunks, MaxRightBaseChunks + 1);
        int FirstRightForestChunks = random.Next(MinRightForestChunks, MaxRightForestChunks + 1);
        int SecondRightForestChunks = random.Next(MinRightForestChunks, MaxRightForestChunks + 1);
        int RightIntermediaryChunks = random.Next(MinRightIntermediaryChunks, MaxRightIntermediaryChunks + 1);
        currentChunk = Instantiate(baseChunks[0], new Vector3(0, -1.6f, 0), Quaternion.identity);
        currentChunk.transform.parent = parentObject.transform;
        currentChunk.GetComponent<PropRandomizer>().SpawnProps();
        spawnedChunks.Add(currentChunk);
        currentChunkType = currentChunk.GetComponent<ChunkProperties>().chunkProperties.Type;
        previousChunkType = currentChunkType;
        GameObject lastChunk = currentChunk;
        GenerateArea("Left", ref lastChunk, LeftBaseChunks, baseChunks);
        GenerateArea("Left", ref lastChunk, FirstLeftForestChunks, forestChunks);
        GenerateArea("Left", ref lastChunk, LeftIntermediaryChunks, baseChunks);
        GenerateArea("Left", ref lastChunk, SecondLeftForestChunks, forestChunks);
        GenerateArea("Left", ref lastChunk, 1, baseChunks);
        GenerateArea("Left", ref lastChunk, 1, marginChunks);
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        float mainCameraHalfHeight;
        float mainCameraHalfWidth;
        mainCameraHalfHeight = camera.orthographicSize;
        mainCameraHalfWidth = mainCameraHalfHeight * camera.aspect;
        float spriteWidth = lastChunk.transform.Find("Floor").GetComponent<SpriteRenderer>().bounds.size.x;
        leftMostX = lastChunk.transform.position.x - spriteWidth / 2 + mainCameraHalfWidth;
        lastChunk = currentChunk;
        GenerateArea("Right", ref lastChunk, RightBaseChunks, baseChunks);
        GenerateArea("Right", ref lastChunk, FirstRightForestChunks, forestChunks);
        GenerateArea("Right", ref lastChunk, RightIntermediaryChunks, baseChunks);
        GenerateArea("Right", ref lastChunk, SecondRightForestChunks, forestChunks);
        GenerateArea("Right", ref lastChunk, 1, baseChunks);
        GenerateArea("Right", ref lastChunk, 1, marginChunks);
        lastChunk.transform.localScale = new Vector3(-lastChunk.transform.localScale.x, lastChunk.transform.localScale.y, lastChunk.transform.localScale.z);
        rightMostX = lastChunk.transform.position.x + spriteWidth / 2 - mainCameraHalfWidth;
        CameraMovement cameraMov = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
        cameraMov.leftBound = leftMostX;
        cameraMov.rightBound = rightMostX;
        cameraMov = GameObject.Find("Camera").GetComponent<CameraMovement>();
        cameraMov.leftBound = leftMostX;
        cameraMov.rightBound = rightMostX;
    }
    void GenerateArea(string staticPoint,ref GameObject lastChunk, int nrChunks, List<GameObject> chunkPool) 
    {
        Vector3 chunkPosition;
        GameObject current;
        System.Random random = new System.Random();
        for (int i = 0; i < nrChunks; i++)
        {
            chunkPosition = lastChunk.transform.Find(staticPoint).position;
            current = Instantiate(chunkPool[random.Next(0, chunkPool.Count)], chunkPosition, Quaternion.identity);
            current.transform.parent = lastChunk.transform.parent;
            current.GetComponent<PropRandomizer>().SpawnProps();
            spawnedChunks.Add(current);
            lastChunk = current;
        }
    }

    public void LoadData(GameData gameData)
    {
        DataPersistenceManager dataPersistanceManager = FindObjectOfType<DataPersistenceManager>();
        foreach (ChunkPropertiesUtils chunkProperties in gameData.chunks)
        {
            GameObject newChunk = Instantiate(dataPersistanceManager.GetPrefabFromGUID(chunkProperties.chunkGUID), chunkProperties.position, Quaternion.identity);
            newChunk.transform.parent = parentObject.transform;
            newChunk.GetComponent<PropRandomizer>().SpawnPropsFromList(chunkProperties.propsGUIDs, dataPersistanceManager);
            spawnedChunks.Add(newChunk);
        }
        currentChunk = spawnedChunks[gameData.CurrentChunkIndex];
        currentChunkType = currentChunk.GetComponent<ChunkProperties>().chunkProperties.Type;
        previousChunkType = currentChunkType;
        if(gameData.LeftBoundary != 0)  // if they are initialized from previous save
        {
            leftMostX = gameData.LeftBoundary;
            rightMostX = gameData.RightBoundary;
        }
        CameraMovement cameraMov = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
        cameraMov.leftBound = leftMostX;
        cameraMov.rightBound = rightMostX;
        cameraMov = GameObject.Find("Camera").GetComponent<CameraMovement>();
        cameraMov.leftBound = leftMostX;
        cameraMov.rightBound = rightMostX;
    }

    public void SaveData(GameData gameData)
    {
        if(gameData.chunks.Count == 0)
        {
            foreach (GameObject chunk in spawnedChunks)
            {
                ChunkProperties chunkProp = chunk.GetComponent<ChunkProperties>();
                gameData.chunks.Add(new ChunkPropertiesUtils
                {
                    position = chunk.transform.position,
                    chunkGUID = chunkProp.chunkPrefabGUID,
                    propsGUIDs = chunkProp.GetChunkPropPrefabsGUIDs()
                });
            }
        }
        gameData.LeftBoundary = leftMostX;
        gameData.RightBoundary = rightMostX;
    }
}
