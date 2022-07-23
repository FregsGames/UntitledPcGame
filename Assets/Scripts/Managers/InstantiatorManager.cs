using UnityEngine;
using static App;

public class InstantiatorManager : Singleton<InstantiatorManager>
{
    [SerializeField]
    private PrefabManager prefabManager;
    [SerializeField]
    private FolderManager folderManager;
    [SerializeField]
    private AppsRequirenments appRequirenments;
    [SerializeField]
    private SystemEventManager systemEventManager;

    public GameObject Instantiate(AppType type, string id = "")
    {
        if (appRequirenments.RequiresAdmin(type) && !Computer.Instance.ComputerSettings.AdminEnabled)
        {
            systemEventManager.RequestPopUp("Necesitas permisos de administrador para abrir este programa", App.AppType.OkPopup);
            return null;
        }

        if (appRequirenments.RequiresWifi(type) && !Computer.Instance.ComputerSettings.WifiEnabled)
        {
            systemEventManager.RequestPopUp("Necesitas conexión para abrir este programa", App.AppType.OkPopup);
            return null;
        }

        if (LockManager.Instance.IsLocked(type))
        {
            LockManager.Instance.ResolveOpenAttempt(type);
            return null;
        }

        if (LockManager.Instance.IsLocked(id, true))
        {
            LockManager.Instance.ResolveOpenAttempt(id, true);
            return null;
        }

        switch (type)
        {
            case AppType.LockedFolder:
                return folderManager.OpenFolder(id, lockedFolder: true);
            case AppType.Folder:
                return folderManager.OpenFolder(id, lockedFolder: false);
            default:
                return prefabManager.InstantiatePrefab(type, id);
        }
    }

}
