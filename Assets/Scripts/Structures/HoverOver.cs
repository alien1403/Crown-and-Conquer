using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverOver : MonoBehaviour
{
    public float hoverScaleFactor = 1.1f; // Adjust this value to control the scale factor on hover
    public float scaleSpeed = 2f; // Adjust this value to control the speed of the scaling transition
    public Color hoverTintColor = new Color(1f, 1f, 1f, 0.5f); // Adjust this value to control the tint color and alpha
    private Vector3 originalScale;
    private bool isHovered = false;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    [AllowNull]
    public GameObject UpgradePanel;
    public GameObject[] buildingLevels;
    public int nextLevel = 1;
    public GameObject UpgradeEffect; 



    void Start()
    {
        // Store the original scale and color of the object
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

    void Update()
    {
        // Smoothly interpolate the scale based on whether the mouse is hovering
        float targetScale = isHovered ? hoverScaleFactor : 1.0f;
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale * targetScale, Time.deltaTime * scaleSpeed);

        // Smoothly interpolate the color tint based on whether the mouse is hovering
        Color targetColor = isHovered ? hoverTintColor : originalColor;
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * scaleSpeed);
    }

    void OnMouseEnter()
    {
        // Set the flag to indicate that the mouse is hovering
        if (nextLevel <= 3)
        {
            isHovered = true;
            UpgradePanel.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        // Set the flag to indicate that the mouse is not hovering
        isHovered = false;
        UpgradePanel.SetActive(false); 
    }

    void OnMouseDown()
    {
        StartCoroutine(UpgradeWithEffect());
      
    }

    IEnumerator UpgradeWithEffect()
    {
        if (nextLevel <= 3)
        {
            Instantiate(UpgradeEffect, transform.position + new Vector3(0.4f, 0f, 0f), Quaternion.identity);

            yield return new WaitForSeconds(0.5f); // Adjust the delay time as needed

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

   

