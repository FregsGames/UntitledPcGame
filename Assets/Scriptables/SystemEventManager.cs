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

    public bool RequestPopUp(App.AppType appType = App.AppType.ConfirmationPopup)
    {
        if (appType != App.AppType.ConfirmationPopup && appType != App.AppType.StringPopup)
            return false;

        if (Computer.Instance.Ram.IsAppOpen(appType))
            return false;

        InstantiatorManager.Instance.Instantiate(appType);
        return true;
    }
}
