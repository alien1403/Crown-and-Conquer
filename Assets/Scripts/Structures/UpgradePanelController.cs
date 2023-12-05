using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UpgradePanelController : MonoBehaviour
{
    private int nextLevel;
    private BuildingController parentScript;
    public float spacing;
    private int childCount = 0;
    private SpriteRenderer brotherSpriteRenderer;
    private GameObject activeBrother;

    // Start is called before the first frame update
    void Start()
    {
        // daca are copii
        if (transform.childCount != 0)
        {
            childCount = transform.childCount;
            SpreadChildrenEqually();
        }

        // daca are parinte
        if (transform.parent != null)
        {
            parentScript = transform.parent.GetComponent<BuildingController>();    
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (parentScript != null)
        {
            nextLevel = parentScript.nextLevel;
            
            for (int i = 0; i <= nextLevel; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(true);
            }

            activeBrother = GetActiveBrother();

            brotherSpriteRenderer = activeBrother.GetComponent<SpriteRenderer>();

            transform.localPosition = new Vector3(-0.5f, brotherSpriteRenderer.bounds.extents.y + 0.8f, 0f);
        }

    }

    void SpreadChildrenEqually()
    {

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Calculate the new x position based on the index and spacing
            float newX = i * spacing;
           
            // Set the new position for the child
            Vector3 newPosition = new Vector3(newX, 0f, 0f);
            child.localPosition = newPosition;
        }
    }

    GameObject GetActiveBrother()
    {
        // Loop through each sibling
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform sibling = transform.parent.GetChild(i);
             
            // Check if the sibling is active and not the current object
            if (sibling.gameObject.activeSelf && sibling != transform)
            { 
                return sibling.gameObject;
               
            }
        }
        return null;
    }
}
