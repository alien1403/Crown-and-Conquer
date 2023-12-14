using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;


public class TownHallController : BuildingController
{

    void Start()
    {
        TownHallDictator.townHallLevel = 0;
        base.Start();

    }

    void Update()
    {
        base.Update();
    }

    void OnMouseEnter()
    {
        // flag for hovering
        if (nextLevelCost != null)
        {
            isHovered = true;
            upgradePanel.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        base.OnMouseExit();
    }

    void OnMouseDown()
    {
        // upgrade cost logic
        if (nextLevelCost == null ||
            nextLevelCost.CoinCost > inventoryManager.goldCounter ||
            nextLevelCost.WoodCost > inventoryManager.woodCounter ||
            nextLevelCost.StoneCost > inventoryManager.stoneCounter ||
            nextLevelCost.IronCost > inventoryManager.ironCounter)
        {
            return;
        }
        else
        {
            inventoryManager.goldCounter -= nextLevelCost.CoinCost;
            inventoryManager.woodCounter -= nextLevelCost.WoodCost;
            inventoryManager.stoneCounter -= nextLevelCost.StoneCost;
            inventoryManager.ironCounter -= nextLevelCost.IronCost;

            inventoryManager.HandleInventoryChange();
        }

        if (upgradeEffect != null && isHovered == true && upgradePanel.activeSelf == true)
        {
            nextLevelCost = nextLevelCost.nextLevel;
            StartCoroutine(UpgradeWithEffect());
        }

        isHovered = false;
        upgradePanel.SetActive(false);
    }

    IEnumerator UpgradeWithEffect()
    {
        if (nextLevel <= maxLevel)
        {
            Instantiate(upgradeEffect, transform.position + new Vector3(0.4f, 0f, 0f), Quaternion.identity);

            yield return new WaitForSeconds(0.1f); 

            buildingLevels[nextLevel - 1].SetActive(false);
            buildingLevels[nextLevel].SetActive(true);

            if (buildingLevels.Length != 0)
            {
                spriteRenderer = buildingLevels[nextLevel].GetComponent<SpriteRenderer>();
                originalColor = spriteRenderer.color;
            }

            nextLevel++;
            TownHallDictator.townHallLevel++;
        }
    }

}



