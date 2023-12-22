using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;


public class BuildingController : MonoBehaviour
{
    public float hoverScaleFactor = 1.1f; 
    public float scaleSpeed = 2f; 
    public Color hoverTintColor = new Color(1f, 1f, 1f, 0.5f); 
    protected Vector3 originalScale;
    protected bool isHovered = false;
    protected Color originalColor;
    protected SpriteRenderer spriteRenderer;
    public GameObject upgradePanel;
    public GameObject[] buildingLevels;
    public int nextLevel = 1;
    public GameObject upgradeEffect;
    [SerializeField]
    protected int maxLevel;
    public BuildingScriptableObject nextLevelCost;
    protected InventoryManager inventoryManager;

    protected void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        originalScale = transform.localScale;

        if (buildingLevels.Length == 0)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
        }
        else
        {
            spriteRenderer = buildingLevels[0].GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
        }

    }

    protected void Update()
    {
        // increase in scale
        float targetScale = isHovered ? hoverScaleFactor : 1.0f;
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale * targetScale, Time.deltaTime * scaleSpeed);

        // red color change
        Color targetColor = isHovered ? hoverTintColor : originalColor;
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * scaleSpeed);
    }

    void OnMouseEnter()
    {
        // flag for hovering
        if (nextLevelCost != null && nextLevel - 1 < TownHallDictator.townHallLevel)
        {
            isHovered = true;
            upgradePanel.SetActive(true);
        }
    }

    protected void OnMouseExit()
    {
        // flag for hovering
        isHovered = false;
        upgradePanel.SetActive(false); 
    }

    void OnMouseDown()
    {
       
        // upgrade cost logic
        if ( nextLevelCost == null ||
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

        if (upgradeEffect != null && nextLevel - 1 < TownHallDictator.townHallLevel && isHovered == true && upgradePanel.activeSelf == true)
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

            yield return new WaitForSeconds(0.5f); 

            buildingLevels[nextLevel - 1].SetActive(false);
            buildingLevels[nextLevel].SetActive(true);

            if (buildingLevels.Length != 0)
            {
                spriteRenderer = buildingLevels[nextLevel].GetComponent<SpriteRenderer>();
                originalColor = spriteRenderer.color;
            }

            nextLevel++;

        }
    }

}

   

