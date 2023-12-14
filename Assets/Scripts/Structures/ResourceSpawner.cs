using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject resourceToSpawn;
    public GameObject player;
    private BuildingController buildingController;
    public float spawnIntervalForLevel1;
    public float spawnIntervalForLevel2;
    public float spawnIntervalForLevel3;
    private float timer = 0f;
    private bool condition;
    private int resourcesGenerated = 0;

    void Start()
    {
        buildingController = GetComponent<BuildingController>();
    }

    void Update()
    {
        if (buildingController.nextLevel > 1)
        {
            timer += Time.deltaTime;

            if(buildingController.nextLevel == 2)
                condition = timer >= spawnIntervalForLevel1;
            else if(buildingController.nextLevel == 3)
                condition = timer >= spawnIntervalForLevel2;
            else
                condition = timer >= spawnIntervalForLevel3;

            if (condition)
            {
                timer = 0f;
                resourcesGenerated++;
            }
            
            if(PlayerIsClose())
            {
                while (resourcesGenerated != 0)
                {                    
                    Vector3 positionOffset = new Vector3(Random.Range(-0.6f, 0.6f), -0.7f, -1f);
                    Instantiate(resourceToSpawn, transform.position + positionOffset, Quaternion.identity);
                    resourcesGenerated--;
                }
            }
        }
    }

    bool PlayerIsClose()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 resourceBuildingPosition = transform.position;
        
        float distance = Vector3.Distance(playerPosition, resourceBuildingPosition);

        return distance <= 2f;
       
    }

}
