using UnityEngine;

public class ComputerSettings : MonoBehaviour
{
    [SerializeField]
    public int maxSubfolderLevel = 5;
    [SerializeField]
    private SystemEventManager eventManager;

    [Header("Debug")]
    [SerializeField]
    private bool wifiEnabled = false;
    [SerializeField]
    private bool adminEnabled = false;

    public int MaxSubfolderLevel { get => maxSubfolderLevel; set => maxSubfolderLevel = value; }

    public bool WifiEnabled { get => wifiEnabled; private set => wifiEnabled = value; }
    public bool AdminEnabled { get => adminEnabled; private set => adminEnabled = value; }

    public void Initialize()
    {
        wifiEnabled = SaveManager.Instance.RetrieveInt("settings_wifi_enabled", 0) == 1;
        adminEnabled = SaveManager.Instance.RetrieveInt("settings_admin_enabled", 0) == 1;

        if (wifiEnabled)
        {
            eventManager.OnWifiEnabled?.Invoke(true);
        }
    }

    public void SetAdminEnabled(bool state)
    {
        adminEnabled = state;
        SaveManager.Instance.Save("settings_admin_enabled", state ? 1 : 0);
    }

    public void SetWifiEnabled(bool state)
    {
        wifiEnabled = state;
        SaveManager.Instance.Save("settings_wifi_enabled", state ? 1 : 0);
        eventManager.OnWifiEnabled?.Invoke(state);
    }
}
