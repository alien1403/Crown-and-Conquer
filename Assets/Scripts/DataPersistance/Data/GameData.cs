using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public List<ChunkPropertiesUtils> chunks = new List<ChunkPropertiesUtils>();
    public int CurrentChunkIndex;
    public int GoldCounter;
    public int WoodCounter;
    public int StoneCounter;
    public int IronCounter;
    public GameData()
    {
        playerPosition = new Vector3(0f, -1f, 0f);
    }
}
