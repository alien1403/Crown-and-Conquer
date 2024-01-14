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
    [SerializeField]
    protected GameObject upgradeEffect;
    [SerializeField]
    public BuildingScriptableObject nextLevelScriptableObject;
    protected InventoryManager inventoryManager;
    public int currentLevel = 0;
    protected UpgradePanelController upgradePanelController;

    protected void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        upgradePanelController = upgradePanel.GetComponent<UpgradePanelController>();
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

    protected virtual void OnMouseEnter()
    {
        // flag for hovering
        if (nextLevelScriptableObject != null)
        {
            isHovered = true;
            upgradePanel.SetActive(true);
            SampleSceneController._instance.ToggleInventoryPanel(true);
        }
    }

    protected void OnMouseExit()
    {
        // flag for hovering
        isHovered = false;
        upgradePanel.SetActive(false);
        SampleSceneController._instance.ToggleInventoryPanel(false);
    }

    protected virtual void OnMouseDown()
    {
        if (CheckUpgrade())
        {
            return;
        }
        TakeResourcesFromPlayerInventory();
        inventoryManager.HandleInventoryChange();
        nextLevelScriptableObject = nextLevelScriptableObject.nextLevel;

        if (upgradeEffect != null)
        {
            StartCoroutine(UpgradeWithEffect());
        }
        else
        {
            SwitchToNextLevel();
        }
        isHovered = false;
        upgradePanel.SetActive(false);
        SampleSceneController._instance.ToggleInventoryPanel(false);
        if(nextLevelScriptableObject != null)
        {
            upgradePanelController.UpdatePanel();

        }
    }

    protected IEnumerator UpgradeWithEffect()
    {
        Instantiate(upgradeEffect, transform.position + new Vector3(0.4f, 0f, 0f), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
        SwitchToNextLevel();
    }

    // returns false if player has enough resources to upgrade
    protected bool CheckUpgrade()
    {
        return nextLevelScriptableObject == null ||
                nextLevelScriptableObject.CoinCost > inventoryManager.goldCounter ||
                nextLevelScriptableObject.WoodCost > inventoryManager.woodCounter ||
                nextLevelScriptableObject.StoneCost > inventoryManager.stoneCounter ||
                nextLevelScriptableObject.IronCost > inventoryManager.ironCounter;
    }
    protected void TakeResourcesFromPlayerInventory()
    {
        inventoryManager.goldCounter -= nextLevelScriptableObject.CoinCost;
        inventoryManager.woodCounter -= nextLevelScriptableObject.WoodCost;
        inventoryManager.stoneCounter -= nextLevelScriptableObject.StoneCost;
        inventoryManager.ironCounter -= nextLevelScriptableObject.IronCost;
    }
    protected void SwitchToNextLevel()
    {
        buildingLevels[currentLevel].SetActive(false);
        currentLevel++;
        buildingLevels[currentLevel].SetActive(true);
    }

}



