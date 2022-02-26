using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Prefabs DB", menuName = "ScriptableObjects/Prefabs DB", order = 3)]
public class PrefabsDB : SerializedScriptableObject
{
    public enum AppType 
    { 
        Folder,
        TextFile,
        Desktop
    }

    [SerializeField]
    private Dictionary<AppType, GameObject> prefabs;

    public GameObject GetPrefab(AppType type)
    {
        if (prefabs.ContainsKey(type))
        {
            return prefabs[type];
        }
        else
        {
            return null;
        }
    }

    public AppType GetID(GameObject prefab)
    {
        return prefabs.FirstOrDefault(s => s.Value == prefab).Key;
    }
}
