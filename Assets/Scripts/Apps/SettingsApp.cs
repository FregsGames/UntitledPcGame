using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsApp : App, IStateableApp
{
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private TextMeshProUGUI volumeText;
    [SerializeField]
    private GameObject adminNotEnabledContainer;
    [SerializeField]
    private GameObject wifiContainer;
    [SerializeField]
    private Toggle wifiToggle;

    [SerializeField]
    private AppStateDictionary states;
    public string StateFamilyName { get => "SettingState"; }
    public Enum CurrentState { get => AppsStates.SettingState.Main; set => CurrentState = value; }
    public AppStateDictionary States => states;

    public void LoadState(Enum state)
    {
        if (state is not AppsStates.SettingState)
            return;

        foreach (var s in States.States)
        {
            s.Value.SetActive(s.Key == state);
        }
    }

    public void EnableAdmin()
    {
        if (LockManager.Instance.IsLocked("admin", false))
        {
            LockManager.Instance.ResolveOpenAttempt("admin", false);
        }
        else
        {
            Computer.Instance.ComputerSettings.SetAdminEnabled(true);
        }

    }

    public void ToggleWifi(bool state)
    {
        if (state && !Computer.Instance.ComputerSettings.WifiPassKnown && LockManager.Instance.IsLocked("wifi", false))
        {
            wifiToggle.SetIsOnWithoutNotify(false);
            LockManager.Instance.ResolveOpenAttempt("wifi", false);
        }
        else
        {
            Computer.Instance.ComputerSettings.SetWifiEnabled(state);
        }
    }

    private void OnEnable()
    {
        soundSlider.SetValueWithoutNotify(SoundManager.Instance.Volume);
        volumeText.text = $"{Mathf.Round(SoundManager.Instance.Volume * 100)}%";
        soundSlider.onValueChanged.AddListener(AdjustVolume);

        adminNotEnabledContainer.SetActive(!Computer.Instance.ComputerSettings.AdminEnabled);
        wifiContainer.SetActive(Computer.Instance.ComputerSettings.AdminEnabled);

        wifiToggle.SetIsOnWithoutNotify(Computer.Instance.ComputerSettings.WifiEnabled);
        wifiToggle.onValueChanged.AddListener(ToggleWifi);

        systemEventManager.OnOtherUnlocked += OnOtherUnlocked;
        systemEventManager.OnAdminEnabled += ShowAdminOptions;
    }

    private void ShowAdminOptions(bool state)
    {
        adminNotEnabledContainer.SetActive(!state);
        wifiContainer.SetActive(state);
    }

    private void OnOtherUnlocked(string key)
    {
        if (key == "wifi")
        {
            wifiToggle.SetIsOnWithoutNotify(true);
        }

        if (key == "admin")
        {
            Computer.Instance.ComputerSettings.SetAdminEnabled(true);
        }
    }

    private void OnDisable()
    {
        soundSlider.onValueChanged.RemoveAllListeners();
        wifiToggle.onValueChanged.RemoveAllListeners();
        systemEventManager.OnOtherUnlocked -= OnOtherUnlocked;
        systemEventManager.OnAdminEnabled -= ShowAdminOptions;
    }

    public void AdjustVolume(float vol)
    {
        SoundManager.Instance.SetVolume(vol);
        volumeText.text = $"{Mathf.Round(SoundManager.Instance.Volume * 100)}%";
    }
}
