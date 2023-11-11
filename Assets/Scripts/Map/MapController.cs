using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static ChunkPropertiesScriptableObject;

public class MapController : MonoBehaviour
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

    [HideInInspector]
    public float leftMostX;
    [HideInInspector]
    public float rightMostX;
    void Start()
    {
        currentChunkType = currentChunk.GetComponent<ChunkProperties>().chunkProperties.Type;
        previousChunkType = currentChunkType;
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
        GenerateWorld();
    }

    void Update()
    {
        //ChunkChecker();
        ChunkOptimizer();
    }
    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }
        if (playerMovement.moveDirection.x > 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }
        if (playerMovement.moveDirection.x < 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
    }
    void SpawnChunk()
    {
        int rand = UnityEngine.Random.Range(0, baseChunks.Count);
        latestSpawnedChunk = Instantiate(baseChunks[rand], noTerrainPosition, Quaternion.identity);
        latestSpawnedChunk.transform.parent = currentChunk.transform.parent;
        spawnedChunks.Add(latestSpawnedChunk);
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
        GameObject lastChunk = currentChunk;
        GenerateArea("Left", ref lastChunk, LeftBaseChunks, baseChunks);
        GenerateArea("Left", ref lastChunk, FirstLeftForestChunks, forestChunks);
        GenerateArea("Left", ref lastChunk, LeftIntermediaryChunks, baseChunks);
        GenerateArea("Left", ref lastChunk, SecondLeftForestChunks, forestChunks);
        GenerateArea("Left", ref lastChunk, 1, baseChunks);
        GenerateArea("Left", ref lastChunk, 1, marginChunks);
        float spriteWidth = lastChunk.transform.Find("Floor").GetComponent<SpriteRenderer>().bounds.size.x;
        leftMostX = lastChunk.transform.position.x - spriteWidth / 2;
        lastChunk = currentChunk;
        GenerateArea("Right", ref lastChunk, RightBaseChunks, baseChunks);
        GenerateArea("Right", ref lastChunk, FirstRightForestChunks, forestChunks);
        GenerateArea("Right", ref lastChunk, RightIntermediaryChunks, baseChunks);
        GenerateArea("Right", ref lastChunk, SecondRightForestChunks, forestChunks);
        GenerateArea("Right", ref lastChunk, 1, baseChunks);
        GenerateArea("Right", ref lastChunk, 1, marginChunks);
        lastChunk.transform.localScale = new Vector3(-lastChunk.transform.localScale.x, lastChunk.transform.localScale.y, lastChunk.transform.localScale.z);
        rightMostX = lastChunk.transform.position.x + spriteWidth / 2;
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
            spawnedChunks.Add(current);
            lastChunk = current;
        }
    }
}
