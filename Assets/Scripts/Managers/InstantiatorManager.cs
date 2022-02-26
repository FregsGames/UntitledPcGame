using UnityEngine;
using static PrefabsDB;

public class InstantiatorManager : Singleton<InstantiatorManager>
{
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
                return null;
            case AppType.Desktop:
                return null;
            default:
                return null;
        }
    }

}
