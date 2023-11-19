using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ChunkProperties : MonoBehaviour
{
    public ChunkPropertiesScriptableObject chunkProperties;
    public string chunkPrefabGUID;
    public List<string> GetChunkPropPrefabsGUIDs()
    {
        List<string> list = new List<string>();
        Transform props = transform.GetChild(4);
        if(props.name == "Props")
        {
            for (int i = 0; i < props.childCount; i++)
            {
                Transform propLocation = props.GetChild(i);
                list.Add(propLocation.GetChild(0).GetComponent<PropProperties>().PrefabGUID);
            }
        }
        return list;
    }
}
