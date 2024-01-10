using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefab;  // Drag your Portal Prefab here in the Inspector
    public GameObject portalPrefab2;
    private bool portalSpawned = false;
    private Transform leftCliff;
    private Transform rightCliff;

    void Start()
    {
       /* 
        if(!portalSpawned)
        {
            //SpawnPortal();

            GameObject floorsObject = GameObject.Find("Floors");

            if (floorsObject != null)
            {
                foreach (Transform child in floorsObject.transform)
                {
                    if (child.name.Contains("Cliff"))
                    {
                        Transform backgroundChild = child.Find("Background");
                        if (backgroundChild != null)
                        {
                            float childX = backgroundChild.position.x;

                            if (childX < 0)
                            {
                                if (leftCliff == null || childX > leftCliff.position.x)
                                {
                                    leftCliff = backgroundChild;
                                }
                            }
                            else
                            {
                                if (rightCliff == null || childX < rightCliff.position.x)
                                {
                                    rightCliff = backgroundChild;
                                }
                            }
                        }
                    }
                }
            }
            //Vector3 spawnPosition = new Vector3(-7f, 0f, 0f);  // Set the desired spawn position
            if (leftCliff != null || rightCliff != null)
            {
                Vector3 spawnPosition = leftCliff.position;
                spawnPosition += Vector3.forward * Random.Range(-3f, 3f);


                // Instantiate the portal prefab at the spawn position
                GameObject portalInstance = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);
                portalSpawned = true;

                //Vector3 spawnPosition2 = new Vector3(0f, 0f, 0f);  // Set the desired spawn position
                Vector3 spawnPosition2 = rightCliff.position;
                spawnPosition2 += Vector3.forward * Random.Range(-3f, 3f);

                // Instantiate the portal prefab at the spawn position
                GameObject portalInstance2 = Instantiate(portalPrefab2, spawnPosition2, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Nu e bine pentrru ca e null");
            }
       
        }
       */ 
    }

    IEnumerator SearchForCliffs()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject floorsObject = GameObject.Find("Floors");

        if (floorsObject != null)
        {
            foreach (Transform child in floorsObject.transform)
            {
                if (child.name.Contains("Cliff"))
                {
                    Transform backgroundChild = child.Find("Background");
                    if (backgroundChild != null)
                    {
                        float childX = backgroundChild.position.x;

                        if (childX < 0)
                        {
                            if (leftCliff == null || childX > leftCliff.position.x)
                            {
                                leftCliff = backgroundChild;
                            }
                        }
                        else
                        {
                            if (rightCliff == null || childX < rightCliff.position.x)
                            {
                                rightCliff = backgroundChild;
                            }
                        }
                    }
                }
            }
        }
    }

    /*
    void SpawnPortal()
    {
        // Set the spawn position


        // Optionally, you can do additional setup or modifications to the spawned portal instance
        // For example, you might want to set its properties or attach it to a specific parent object.

        // portalInstance.transform.parent = someParentGameObject;  // Set a parent if needed
    }
    */
}
