using System;
using UnityEngine;

[CreateAssetMenu(fileName = "System Event Manager", menuName = "ScriptableObjects/System Event Manager", order = 4)]
public class SystemEventManager : ScriptableObject
{
    public Action<App> OnAppOpen;
    public Action<string> OnAppClosed;
    public Action OnLanguagueLoaded;
    
    public Action<bool> OnPopUpSubmit;
    public Action<string> OnStringPopUpSubmit;
    public Action<bool> OnNumericPopUpSubmit;

    public App RequestPopUp(string text, App.AppType appType)
    {
        if (!appType.ToString().Contains("Popup",StringComparison.OrdinalIgnoreCase))
            return null;

        if (Computer.Instance.Ram.IsAppOpen(appType))
            return null;

        GameObject gameObject = InstantiatorManager.Instance.Instantiate(appType);
        gameObject.GetComponent<IPopUp>().SetText(text);
        return gameObject.GetComponent<App>();
    }
}
