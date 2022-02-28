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

    public GameObject InstantiatePrefab(AppType type)
    {
        GameObject toInstantiate = prefabsDB.GetPrefab(type);
        return  Instantiate(toInstantiate, computerScreen.transform);
    }
}
