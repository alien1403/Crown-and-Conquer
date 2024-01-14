using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IDataPersistence
{
    public static EnemyManager instance { get; private set; }
    public List<GameObject> enemiesPortals = new List<GameObject>();
    [HideInInspector]
    public bool enemiesSpawnedInCurrentDay;

    private WorldTime worldTime;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    [Header("Portal Config")]
    [SerializeField]
    private Transform enemiesTargetParent;
    [Header("Enemies")]
    [SerializeField]
    private List<GameObjectProbability> enemyPrefabs;
    [SerializeField]
    private float floorOffset = -1.44f;

    private void Awake()
    {
        instance = this;
        worldTime = FindObjectOfType<WorldTime>();
        GameObjectProbability.NormalizeProbabilities(ref enemyPrefabs);
    }
    public void SpawnEnemies()
    {
        if(enemiesSpawnedInCurrentDay)
        {
            return;
        }
        int day = worldTime.dayCount;
        int numberOfEnemies = NumberOfEnemiesGenerator(day);
        List<GameObject> enemiesToSpawn = new List<GameObject>();
        switch (day % 10)
        {
            case 0:
                GenerateRedNight(numberOfEnemies, ref enemiesToSpawn);
                break;
            case 1:
                return;
            default:
                GenerateWhiteNight(numberOfEnemies, ref enemiesToSpawn);
                break;
        }
        foreach(GameObject portal in enemiesPortals)
        {
            StartCoroutine(SpawnEnemiesOneAtATime(enemiesToSpawn, portal.transform.position));
        }
        enemiesSpawnedInCurrentDay = true;
    }
    private int NumberOfEnemiesGenerator(int day)
    {
        float value = Mathf.Pow(day, 4.0f / 5.0f);
        return Mathf.RoundToInt(value);
    }
    private void GenerateWhiteNight(int numberOfEnemies, ref List<GameObject> enemiesToSpawn)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float randomValue = Random.Range(0f, 1f);
            GameObject enemyForSpawning = GameObjectProbability.GetCorrespondentGameObject(ref enemyPrefabs, randomValue);
            while (enemyForSpawning.GetComponent<EnemyController>().enemyScriptableObject.Difficulty == EnemyDifficulty.Hard)
            {
                randomValue = Random.Range(0f, 1f);
                enemyForSpawning = GameObjectProbability.GetCorrespondentGameObject(ref enemyPrefabs, randomValue);
            }
            enemiesToSpawn.Add(enemyForSpawning);
        }
    }
    private void GenerateRedNight(int numberOfEnemies, ref List<GameObject> enemiesToSpawn)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float randomValue = Random.Range(0f, 1f);
            GameObject enemyForSpawning = GameObjectProbability.GetCorrespondentGameObject(ref enemyPrefabs, randomValue);
            enemiesToSpawn.Add(enemyForSpawning);
        }
    }
    private IEnumerator SpawnEnemiesOneAtATime(List<GameObject> enemiesToSpawn, Vector3 spawnLocation)
    {
        foreach (GameObject enemy in enemiesToSpawn)
        {
            float prefabHeight = enemy.GetComponent<Renderer>().bounds.size.y;
            float offset = prefabHeight / 2f;
            Vector3 spawnPosition = new Vector3(spawnLocation.x, floorOffset + offset, spawnLocation.z);
            GameObject spawnedEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);

            spawnedEnemy.transform.parent = enemiesTargetParent;
            EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
            enemyController.SpawnDay = worldTime.dayCount;
            enemyController.spawnLocation = spawnLocation;
            spawnedEnemies.Add(spawnedEnemy);
            yield return new WaitForSeconds(1f);
        }
    }
    public void LoadData(GameData gameData)
    {
        DataPersistenceManager manager = DataPersistenceManager.instance;
        foreach (EnemyPropertiesUtils enemyProperty in gameData.enemies)
        {
            GameObject enemyPrefab = manager.GetPrefabFromGUID(enemyProperty.enemyGUID);
            GameObject spawnedEnemy = Instantiate(enemyPrefab, enemyProperty.position, Quaternion.identity);
            spawnedEnemy.transform.parent = enemiesTargetParent;
            EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
            enemyController.SpawnDay = enemyProperty.spawnDay;
            enemyController.spawnLocation = enemyProperty.spawnLocation;
            spawnedEnemies.Add(spawnedEnemy);
        }
        enemiesSpawnedInCurrentDay = gameData.enemiesSpawnedInCurrentDay;
    }
    public void SaveData(GameData gameData)
    {
        List<EnemyPropertiesUtils> enemyPropertiesUtils = new List<EnemyPropertiesUtils>();
        foreach (GameObject enemy in spawnedEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyPropertiesUtils.Add(new EnemyPropertiesUtils
            {
                enemyGUID = enemyController.enemyGUID,
                position = enemy.transform.position,
                spawnDay = enemyController.SpawnDay,
                spawnLocation = enemyController.spawnLocation
            });
        }
        gameData.enemies = enemyPropertiesUtils;
        gameData.enemiesSpawnedInCurrentDay = enemiesSpawnedInCurrentDay;
    }
    public void RemoveRefference(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }
}
