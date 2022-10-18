using System;
using UnityEngine;

[CreateAssetMenu(fileName = "System Event Manager", menuName = "ScriptableObjects/System Event Manager", order = 4)]
public class SystemEventManager : ScriptableObject
{
    public Action OnRestart;

    public Action<App> OnAppOpen;
    public Action<string> OnAppClosed;
    public Action OnLanguagueLoaded;

    public Action<bool> OnPopUpSubmit;
    public Action<string> OnStringPopUpSubmit;
    public Action<bool> OnNumericPopUpSubmit;
    public Action<string> OnNumericPopUpNumberSubmit;
    public Action OnPopUpCancel;

    public Action<KeyCode> OnKeyPressed;

    public Action<App.AppType> OnAppUnlocked;
    public Action<string> OnFileUnlocked;
    public Action<string> OnOtherUnlocked;

    public Action<bool> OnWifiEnabled;
    public Action<bool> OnAdminEnabled;

    public Action<string> OnFileDownloadRequest;

    public Action<string> OnMailReceived;

    public Action<bool> OnAntivirusOpened;

    public Action OnCameraOn;
    public Action OnCameraOff;

    public Action<App> OnVirusDefeated;

    public App RequestPopUp(string text, App.AppType appType)
    {
        if (!appType.ToString().Contains("Popup", StringComparison.OrdinalIgnoreCase))
            return null;

        if (Computer.Instance.Ram.IsAppOpen(appType))
            return null;

        SoundManager.Instance.PlaySound(SoundManager.Sound.PopUpOpen);

        GameObject gameObject = InstantiatorManager.Instance.Instantiate(appType);
        gameObject.GetComponent<IPopUp>().SetText(text);
        return gameObject.GetComponent<App>();
    }
}
