using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreesController : MonoBehaviour
{
    
    void Start()
    {
        GameObject floorsObject = GameObject.Find("Floors");
        if(floorsObject != null)
        {
            Transform floorsTransform = floorsObject.transform;

            foreach(Transform floor in floorsTransform)
            {
                if (floor.name.Contains("Forest"))
                {
                    Transform propsChild = floor.Find("Props");

                    if(propsChild != null)
                    {
                        foreach(Transform propLocation in propsChild)
                        {
                            if(propLocation.name.Contains("Prop Location"))
                            {
                                foreach(Transform child in propLocation)
                                {
                                    if (child.name.ToLower().Contains("tree") && !child.name.ToLower().Contains("bush"))
                                    {
                                        AddWoodManager(child.gameObject);
                                        AddClickHandler(child.gameObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        else
        {
            Debug.LogWarning("Error");
        }
    }


    void AddClickHandler(GameObject tree)
    {
        tree.AddComponent<TreeClickHandler>();
    }

    void AddWoodManager(GameObject tree) 
    { 
        if(!tree.GetComponent<TreeWoodManager>())
        {
            tree.AddComponent<TreeWoodManager>();
        }
    }
}

public class TreeClickHandler : MonoBehaviour
{
    private bool canClick = true;
    [SerializeField] public float clickCooldown = 1.0f;

    void OnMouseDown()
    {
        if (canClick)
        {
            TreeWoodManager woodManager = GetComponent<TreeWoodManager>();
            if (woodManager != null)
            {
                woodManager.DecrementWood();
                Debug.Log("Remaining wood: " + woodManager.GetRemainingWood());
                canClick = false;
                StartCoroutine(ClickCooldown());
            }

            TreeResourceSpawner treeResourceSpawner = FindObjectOfType<TreeResourceSpawner>();

            if (woodManager.GetRemainingWood() != 0)
            {
                if (treeResourceSpawner != null)
                {
                    treeResourceSpawner.SpawnResourceNearTree(transform.position);
                }
                else
                {
                    Debug.LogWarning("TreeResourceSpawner script not found!");
                }
            }
        }
    }

    IEnumerator ClickCooldown()
    {
        yield return new WaitForSeconds(clickCooldown);
        canClick = true;
    }
}
