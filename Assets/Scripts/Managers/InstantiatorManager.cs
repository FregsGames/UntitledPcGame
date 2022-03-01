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
            case AppType.TextFile:
            case AppType.LockedTextFile:
                return prefabManager.InstantiatePrefab(type, id);
            case AppType.Desktop:
                return null;
            case AppType.LockedFolder:
                return folderManager.OpenFolder(id, lockedFolder: true);
            case AppType.Folder:
                return folderManager.OpenFolder(id, lockedFolder: false);
            default:
                return null;
        }
    }

}
