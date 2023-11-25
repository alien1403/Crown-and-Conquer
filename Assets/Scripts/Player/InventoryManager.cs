using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    public int GoldCounter;
    public int WoodCounter;
    public int StoneCounter;
    public int IronCounter;
    void Update()
    {
        
    }
    public void LoadData(GameData gameData)
    {
        this.GoldCounter = gameData.GoldCounter;
        this.WoodCounter = gameData.WoodCounter;
        this.StoneCounter = gameData.StoneCounter;
        this.IronCounter = gameData.IronCounter;
    }

    public void SaveData(GameData gameData)
    {
        gameData.GoldCounter = GoldCounter;
        gameData.WoodCounter = WoodCounter;
        gameData.StoneCounter = StoneCounter;
        gameData.IronCounter = IronCounter;
    }
}
