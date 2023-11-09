using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObjectProbability> propPrefabs;
    void Start()
    {
        SpawnProps();
    }

    void Update()
    {
        
    }
    void SpawnProps()
    {
        GameObjectProbability.NormalizeProbabilities(ref propPrefabs);

        foreach (GameObject spawnPoint in propSpawnPoints)
        {
            float randomValue = Random.Range(0f, 1f);
            GameObject spawnedProp = Instantiate(GameObjectProbability.GetCorrespondentGameObject(ref propPrefabs, randomValue), spawnPoint.transform.position, Quaternion.identity);
            spawnedProp.transform.parent = spawnPoint.transform;
        }
    }
}
