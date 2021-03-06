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
        var prefab = Instantiate(toInstantiate, computerScreen.Desktop);

        if (id != string.Empty)
        {
            App appComponent = prefab.GetComponentInChildren<App>();
            appComponent.ID = id;
            appComponent.Deserialize();
        }
        else
        {
            App app = prefab.GetComponentInChildren<App>();
            app.ID = Guid.NewGuid().ToString();
            app.Open();
        }

        return prefab;
    }
}
