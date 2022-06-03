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
            case AppType.ConfirmationPopup:
            case AppType.StringPopup:
            case AppType.NumericPopup:
            case AppType.Settings:
                return prefabManager.InstantiatePrefab(type, id);
            case AppType.LockedFolder:
                return folderManager.OpenFolder(id, lockedFolder: true);
            case AppType.Folder:
                return folderManager.OpenFolder(id, lockedFolder: false);
            case AppType.Desktop:
            default:
                return null;
        }
    }

}
