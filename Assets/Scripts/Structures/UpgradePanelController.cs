using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class UpgradePanelController : MonoBehaviour
{
    private int nextLevel;
    private TownHallController townHallScript;
    private BuildingController parentScript;
    public float spacing;
    private int childCount = 0;
    private SpriteRenderer brotherSpriteRenderer;
    private GameObject activeBrother;
    [SerializeField]
    private TextMeshPro coinCounter;
    [SerializeField]
    private TextMeshPro woodCounter;
    [SerializeField]
    private TextMeshPro stoneCounter;
    [SerializeField]
    private TextMeshPro ironCounter;

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
            townHallScript = transform.parent.GetComponent<TownHallController>();
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

            coinCounter.text = parentScript.nextLevelCost.CoinCost.ToString();
            woodCounter.text = parentScript.nextLevelCost.WoodCost.ToString();
            stoneCounter.text = parentScript.nextLevelCost.StoneCost.ToString();
            ironCounter.text = parentScript.nextLevelCost.IronCost.ToString();

            activeBrother = GetActiveBrother();

            brotherSpriteRenderer = activeBrother.GetComponent<SpriteRenderer>();

            transform.localPosition = new Vector3(-0.5f, brotherSpriteRenderer.bounds.extents.y + 0.8f, 0f);
        }
        else if (townHallScript != null) 
        {
            nextLevel = townHallScript.nextLevel;

            for (int i = 0; i <= nextLevel; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(true);
            }

            coinCounter.text = townHallScript.nextLevelCost.CoinCost.ToString();
            woodCounter.text = townHallScript.nextLevelCost.WoodCost.ToString();
            stoneCounter.text = townHallScript.nextLevelCost.StoneCost.ToString();
            ironCounter.text = townHallScript.nextLevelCost.IronCost.ToString();

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

            float newX = i * spacing;

            Vector3 newPosition = new Vector3(newX, 0f, 0f);
            child.localPosition = newPosition;
        }
    }

    GameObject GetActiveBrother()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform sibling = transform.parent.GetChild(i);
             
            if (sibling.gameObject.activeSelf && sibling != transform)
            { 
                return sibling.gameObject;
               
            }
        }
        return null;
    }
}
