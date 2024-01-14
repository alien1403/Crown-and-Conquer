using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public float hoverScaleFactor = 1.1f;
    public float scaleSpeed = 2f;
    public Color hoverTintColor = new Color(1f, 1f, 1f, 0.5f);
    protected Vector3 originalScale;
    protected bool isHovered = false;
    protected Color originalColor;
    protected SpriteRenderer spriteRenderer;
    protected InventoryManager inventoryManager;
    private static readonly System.Random random = new System.Random();

    [SerializeField]
    protected GameObject upgradeEffect;

    public bool isChestOpened = false;
    public int chestId;
    public Sprite newImage;

    protected void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        //LoadData();
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
        isHovered = true;
    }

    protected void OnMouseExit()
    {
        // flag for hovering
        isHovered = false;
    }

    private int GetRandomInt(int minValue, int maxValue)
    {
        if (minValue > maxValue)
        {
            int temp = minValue;
            minValue = maxValue;
            maxValue = temp;
        }

        return random.Next(minValue, maxValue + 1);
    }

    protected virtual void OnMouseDown()
    {
        if(!isChestOpened)
        {
            inventoryManager.goldCounter += GetRandomInt(1, 3);
            inventoryManager.HandleInventoryChange();
            StartCoroutine(UpgradeWithEffect());
            isHovered = false;
            isChestOpened = true;
            spriteRenderer.sprite = newImage;
            //SaveData();
        }
    }

    protected IEnumerator UpgradeWithEffect()
    {
        Instantiate(upgradeEffect, transform.position + new Vector3(0.4f, 0f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
    }

    //public void LoadData(GameData gameData)
    //{
    //    isChestOpened = gameData.isChestOpened[chestId];
    //}

    //public void SaveData(GameData gameData)
    //{
    //    gameData.isChestOpened[chestId] = isChestOpened;
    //}
}
