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

    }

    void Update()
    {
        
    }
    public void SpawnProps()
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
    public void SpawnPropsFromList(List<string> propsGUIDs, DataPersistenceManager manager)
    {
        for(int i = 0; i < propSpawnPoints.Count; i++)
        {
            GameObject propPrefab = manager.GetPrefabFromGUID(propsGUIDs[i]);
            GameObject spawnedProp = Instantiate(propPrefab, Vector3.zero, Quaternion.identity);
            float prefabHeight = spawnedProp.GetComponent<Renderer>().bounds.size.y;
            float offset = prefabHeight / 2f;
            spawnedProp.transform.parent = propSpawnPoints[i].transform;
            spawnedProp.transform.localPosition = new Vector3(0, offset, 0);
        }
    }
}
