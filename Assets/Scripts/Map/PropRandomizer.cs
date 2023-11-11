using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
            GameObject spawnedProp = GameObjectProbability.GetCorrespondentGameObject(ref propPrefabs, randomValue);
            float prefabHeight = spawnedProp.GetComponent<Renderer>().bounds.size.y;
            float offset = prefabHeight / 2f;
            spawnedProp = Instantiate(spawnedProp, Vector3.zero, Quaternion.identity);
            spawnedProp.transform.parent = spawnPoint.transform;
            spawnedProp.transform.localPosition = new Vector3(0, offset, 0);
        }
    }
}
