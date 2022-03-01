using System;
using UnityEngine;
using static App;

public class PrefabManager : MonoBehaviour
{
    [SerializeField]
    private PrefabsDB prefabsDB;

    private ComputerScreen computerScreen;

    public PrefabsDB PrefabsDB { get => prefabsDB; }

    private void Start()
    {
        computerScreen = ComputerScreen.Instance;
    }

    public GameObject InstantiatePrefab(AppType type, string id = "")
    {
        GameObject toInstantiate = prefabsDB.GetPrefab(type);
        var prefab = Instantiate(toInstantiate, computerScreen.transform);

        if (id != string.Empty)
        {
            prefab.GetComponentInChildren<App>().ID = id;
        }
        else
        {
            prefab.GetComponentInChildren<App>().ID = Guid.NewGuid().ToString();
        }

        return prefab;
    }
}