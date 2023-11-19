using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    MapController mapController;
    [HideInInspector]
    public GameObject targetMap;
    void Start()
    {
        mapController = FindObjectOfType<MapController>();
        targetMap = transform.parent.gameObject;
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mapController.currentChunk = targetMap;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(mapController.currentChunk == targetMap)
            {
                mapController.currentChunk = null;
            }
        }
    }
    
}
