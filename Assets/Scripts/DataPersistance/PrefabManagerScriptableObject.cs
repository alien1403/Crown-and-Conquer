using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PrefabManager : EditorWindow
{
    private const string AssetPath = "Assets/Scriptable Objects/PrefabsDictionaryScriptableObject.asset";
    [MenuItem("Tools/Create Prefab GUID Dictionary")]
    public static void CreatePrefabGUIDDictionary()
    {
        string folderPath = "Assets/Prefabs";

        // Find all prefabs in the specified folder
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        PrefabsDictionaryScriptableObject prefabDictionary = GetOrCreatePrefabDictionaryAsset();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                prefabDictionary.prefabs.Add(guid, prefab);
            }
        }
        EditorUtility.SetDirty(prefabDictionary); // Mark the ScriptableObject as dirty to prompt saving
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private static PrefabsDictionaryScriptableObject GetOrCreatePrefabDictionaryAsset()
    {
        PrefabsDictionaryScriptableObject prefabDictionary = AssetDatabase.LoadAssetAtPath<PrefabsDictionaryScriptableObject>(AssetPath);

        if (prefabDictionary == null)
        {
            prefabDictionary = ScriptableObject.CreateInstance<PrefabsDictionaryScriptableObject>();
            AssetDatabase.CreateAsset(prefabDictionary, AssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        return prefabDictionary;
    }
}
