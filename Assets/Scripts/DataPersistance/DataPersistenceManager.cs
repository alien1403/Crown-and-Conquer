using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistanceObjects;
    private FileDataHandler fileDataHandler;
    public PrefabsDictionaryScriptableObject prefabsDictionary;
    [SerializeField]
    public bool useEncryption;
    public static DataPersistenceManager instance { get; private set; }
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more that one Data Persistance Manager in the scene.");
        }
        instance = this;
    }
    private void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
        FindObjectOfType<MapController>().GenerateWorld();
    }
    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }
        foreach(IDataPersistence dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(gameData);
        }
    }
    public void SaveGame() 
    {
        foreach (IDataPersistence dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.SaveData(gameData);
        }
        fileDataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersistence> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistanceObjects);
    }
    public string GetPrefabGUIDFromInstatiatedGameObject(GameObject instantiatedObject)
    {
        if(instantiatedObject != null)
        {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(instantiatedObject);
            Debug.Log(prefab == null);
            if(prefabsDictionary.PrefabsGUID.ContainsKey(prefab))
                return prefabsDictionary.PrefabsGUID[prefab];
        }
        return string.Empty;
    }
    public GameObject GetPrefabFromGUID(string guid)
    {
        if(prefabsDictionary.GUIDPrefabs.ContainsKey(guid))
        {
            return prefabsDictionary.GUIDPrefabs[guid];
        }
        return null;
    }

    public void DeleteSavedData()
    {
        fileDataHandler.DeleteSavedData();
    }
}
