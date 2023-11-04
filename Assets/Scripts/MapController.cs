using System;
using System.Collections.Generic;
using UnityEngine;
using static ChunkPropertiesScriptableObject;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerMovement playerMovement;
    public GameObject currentChunk;
    [Header("Background")]
    public GameObject CameraTarget;
    public List<BackgroundLayersSpeed> baseBackgroundLayers;
    public List<BackgroundLayersSpeed> forestBackgroundLayers;
    private List<BackgroundLayersSpeed>[] backgroundLayers = new List<BackgroundLayersSpeed>[2];
    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestSpawnedChunk;
    public float maxOpDist;
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDuration;
    [HideInInspector]
    public GameObject latestChunk;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        backgroundLayers[(int)ChunkType.Base] = baseBackgroundLayers;
        backgroundLayers[(int)ChunkType.Forest] = forestBackgroundLayers;
        foreach(int enumValue in Enum.GetValues(typeof(ChunkType)))
        {
            foreach(BackgroundLayersSpeed layer in backgroundLayers[enumValue])
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
        ChunkChecker();
        ChunkOptimizer();
    }
    void ChunkChecker()
    {
        if(!currentChunk)
        {
            return;
        }
        if(playerMovement.moveDirection.x > 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
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
        int rand = UnityEngine.Random.Range(0, terrainChunks.Count);
        latestSpawnedChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestSpawnedChunk);
    }
    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;
        if(optimizerCooldown > 0f)
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
    void CheckBackgroundChunk()
    {
        ChunkType currentType = currentChunk.GetComponent<ChunkProperties>().chunkProperties.Type;
        ChunkType lastType = latestChunk.GetComponent<ChunkProperties>().chunkProperties.Type;

        if (currentType != lastType)
        {
            if(currentType == ChunkType.Base)
            {
                return;
            }
        }
    }
}
