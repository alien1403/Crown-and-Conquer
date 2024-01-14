using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Player stats
    public int GoldCounter = 50;
    public int WoodCounter = 50;
    public int StoneCounter = 50;
    public int IronCounter = 50;
    public Vector3 playerPosition;
    public bool playerFlip;

    // Map variables
    public List<ChunkPropertiesUtils> chunks = new List<ChunkPropertiesUtils>();
    public int CurrentChunkIndex;
    public float LeftBoundary;
    public float RightBoundary;

    // Time variables
    public int DayCount;
    public int Hour;
    public int Minute;

    //Enemies variables
    public List<EnemyPropertiesUtils> enemies = new List<EnemyPropertiesUtils>();
    public bool enemiesSpawnedInCurrentDay;

    public GameData()
    {
        playerPosition = new Vector3(0f, -0.84f, 0f);
        playerFlip = false;
        DayCount = 1;
        Hour = 2;
        Minute = 30;
        LeftBoundary = 0;
        RightBoundary = 0;
        enemiesSpawnedInCurrentDay = false;
    }
}
