using System;
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
    [SerializeField]
    private bool antivirusEnabled = false;
    [SerializeField]
    private bool virusActive = false;

    public int MaxSubfolderLevel { get => maxSubfolderLevel; set => maxSubfolderLevel = value; }

    public bool WifiEnabled { get => wifiEnabled; private set => wifiEnabled = value; }
    public bool AdminEnabled { get => adminEnabled; private set => adminEnabled = value; }
    public bool WifiPassKnown { get; set; }
    public bool AntivirusEnabled { get => antivirusEnabled; set => antivirusEnabled = value; }
    public bool VirusActive { get => virusActive; set => virusActive = value; }

    private void OnEnable()
    {
        eventManager.OnOtherUnlocked += OnOtherUnlocked;
    }

    private void OnOtherUnlocked(string key)
    {
        if(key == "wifi")
        {
            SetWifiEnabled(true);
        }
    }

    private void OnDisable()
    {
        eventManager.OnOtherUnlocked -= OnOtherUnlocked;
    }

    public void Initialize()
    {
        wifiEnabled = SaveManager.Instance.RetrieveInt("settings_wifi_enabled", 0) == 1;
        adminEnabled = SaveManager.Instance.RetrieveInt("settings_admin_enabled", 0) == 1;
        WifiPassKnown = SaveManager.Instance.RetrieveInt("settings_wifiKnown", 0) == 1;

        if (wifiEnabled)
        {
            eventManager.OnWifiEnabled?.Invoke(true);
            WifiPassKnown = true;
            SaveManager.Instance.Save("settings_wifiKnown", 1);
        }
    }

    public void SetAdminEnabled(bool state)
    {
        adminEnabled = state;
        SaveManager.Instance.Save("settings_admin_enabled", state ? 1 : 0);
        eventManager.OnAdminEnabled?.Invoke(state);
    }

    public void SetWifiEnabled(bool state)
    {
        wifiEnabled = state;
        SaveManager.Instance.Save("settings_wifi_enabled", state ? 1 : 0);
        eventManager.OnWifiEnabled?.Invoke(state);

        if (state)
        {
            SaveManager.Instance.Save("settings_wifiKnown", state ? 1 : 0);
        }
    }
}
