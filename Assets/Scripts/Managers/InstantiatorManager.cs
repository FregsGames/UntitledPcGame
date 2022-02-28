using UnityEngine;
using static App;

public class InstantiatorManager : Singleton<InstantiatorManager>
{
    [SerializeField]
    private PrefabManager prefabManager;
    [SerializeField]
    private FolderManager folderManager;

    public GameObject Instantiate(AppType type, string id = "")
    {
        switch (type)
        {
            case AppType.Folder:
                return folderManager.OpenFolder(id);
            case AppType.TextFile:
                return prefabManager.InstantiatePrefab(type);
            case AppType.Desktop:
                return null;
            default:
                return null;
        }
    }

}
