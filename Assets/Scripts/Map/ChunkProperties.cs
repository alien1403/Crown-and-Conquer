using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ChunkType
{
    Forest,
    Base,
}
[System.Serializable]
public class ChunkProperties : MonoBehaviour, IDataPersistence
{
    public ChunkType Type;
    public string Guid;
    public List<string> propsGuids;
    public Vector3 Position;
    private void Awake()
    {
        Guid = DataPersistenceManager.GetPrefabGUID(gameObject);
        Transform propsParent = transform.Find("Props");
        if(propsParent != null )
        {
            for(int i = 0; i < propsParent.childCount; i++)
            {
                propsGuids.Add(DataPersistenceManager.GetPrefabGUID(propsParent.GetChild(i).gameObject));
            }
        }
        Position = this.transform.position;
    }
    public void LoadData(GameData gameData)
    {
        
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.chunkProperties.Add(this);
    }
}
