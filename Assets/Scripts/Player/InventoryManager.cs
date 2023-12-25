using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    public int goldCounter;
    public int woodCounter;
    public int stoneCounter;
    public int ironCounter;

    public TextMeshProUGUI goldCounterText;
    public TextMeshProUGUI woodCounterText;
    public TextMeshProUGUI stoneCounterText;
    public TextMeshProUGUI ironCounterText;
    void Start()
    {

    }
    void Update()
    {
        
    }
    public void LoadData(GameData gameData)
    {
        this.goldCounter = gameData.GoldCounter;
        this.woodCounter = gameData.WoodCounter;
        this.stoneCounter = gameData.StoneCounter;
        this.ironCounter = gameData.IronCounter;
        HandleInventoryChange();
    }
    public void SaveData(GameData gameData)
    {
        gameData.GoldCounter = goldCounter;
        gameData.WoodCounter = woodCounter;
        gameData.StoneCounter = stoneCounter;
        gameData.IronCounter = ironCounter;
    }
    public void HandleInventoryChange()
    {
        goldCounterText.text = goldCounter.ToString();
        woodCounterText.text = woodCounter.ToString();
        stoneCounterText.text = stoneCounter.ToString();
        ironCounterText.text = ironCounter.ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectible collectible))
        {
            collectible.Collect();
            HandleInventoryChange();
        }
    }
}
