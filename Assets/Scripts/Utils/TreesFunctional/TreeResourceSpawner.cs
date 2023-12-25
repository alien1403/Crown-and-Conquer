using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeResourceSpawner : MonoBehaviour
{
    public GameObject resourceToSpawn;

    public void SpawnResourceNearTree(Vector3 treePosition)
    {
        Vector3 positionOffset = new Vector3(Random.Range(-0.5f, 0.5f), -1.25f, 0f);
        treePosition.y = 0;
        Instantiate(resourceToSpawn, treePosition + positionOffset, Quaternion.identity);

        Renderer rend = resourceToSpawn.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.sortingOrder = 5;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the spawned resource!");
        }
    }
}
