using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PrefabDictionaryScriptableObject", menuName ="Custom / Prefabs Dictionary")]
public class PrefabsDictionaryScriptableObject : ScriptableObject
{
    [SerializedDictionary]
    public SerializedDictionary<string, GameObject> prefabs;
}
