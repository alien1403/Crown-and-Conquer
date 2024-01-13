using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class UpgradePanelController : MonoBehaviour
{
    private BuildingController buildingController;
    public float spacing;
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
        //SpreadChildrenEqually();
        buildingController = transform.parent.GetComponent<BuildingController>();
        UpdatePanel();
    }

    // Update is called once per frame
    void Update()
    {

        //for (int i = 0; i <= buildingController.currentLevel + 1; i++)
        //{
        //    Transform child = transform.GetChild(i);
        //    child.gameObject.SetActive(true);
        //}



        //activeBrother = GetActiveBrother();

        //brotherSpriteRenderer = activeBrother.GetComponent<SpriteRenderer>();

        //transform.localPosition = new Vector3(-0.5f, brotherSpriteRenderer.bounds.extents.y + 0.8f, 0f);

    }
    public void UpdatePanel()
    {
        coinCounter.text = buildingController.nextLevelScriptableObject.CoinCost.ToString();
        woodCounter.text = buildingController.nextLevelScriptableObject.WoodCost.ToString();
        stoneCounter.text = buildingController.nextLevelScriptableObject.StoneCost.ToString();
        ironCounter.text = buildingController.nextLevelScriptableObject.IronCost.ToString();
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            TextMeshPro text = child.GetComponentInChildren<TextMeshPro>();
            if(text != null)
            {
                int cost;
                if (int.TryParse(text.text, out cost) && cost != 0)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    void SpreadChildrenEqually()
    {

        for (int i = 0; i < transform.childCount; i++)
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
