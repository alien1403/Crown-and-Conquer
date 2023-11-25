using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ICollectible
{
    public enum ItemType
    {
        Gold,
        Wood,
        Stone,
        Iron
    }
    public ItemType Type;

    public void Collect()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        switch(Type)
        {
            case ItemType.Gold:
                inventoryManager.goldCounter++;
                break;
            case ItemType.Wood:
                inventoryManager.woodCounter++; 
                break;
            case ItemType.Stone:
                inventoryManager.stoneCounter++;
                break;
            case ItemType.Iron:
                inventoryManager.ironCounter++;
                break;
        }
        Destroy(gameObject);
    }
}
