using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : BuildingController
{
    [Header("Wall Settings")]
    [SerializeField]
    public float firstHitThreshold = 0.5f;
    [SerializeField]
    public float secondHitThreshold = 0.0f;
    [HideInInspector]
    public float currentHealth = 0.0f;
    public float maxHealth = 0.0f;
    public bool isDestroyed = false;
    override protected void OnMouseDown()
    {
        if (CheckUpgrade())
        {
            return;
        }
        TakeResourcesFromPlayerInventory();
        inventoryManager.HandleInventoryChange();
        maxHealth = ((WallScriptableObject) nextLevelScriptableObject).Health;
        currentHealth = maxHealth;
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
        if (nextLevelScriptableObject != null)
        {
            upgradePanelController.UpdatePanel();
        }
    }
    public void SwitchToDestroyed()
    {
        isDestroyed = true;
        buildingLevels[currentLevel].SetActive(false);
        buildingLevels[0].SetActive(true);
    }
}
