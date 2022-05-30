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

    public App RequestPopUp(App.AppType appType = App.AppType.ConfirmationPopup)
    {
        if (appType != App.AppType.ConfirmationPopup && appType != App.AppType.StringPopup && appType != App.AppType.NumericPopup)
            return null;

        if (Computer.Instance.Ram.IsAppOpen(appType))
            return null;

        return InstantiatorManager.Instance.Instantiate(appType).GetComponent<App>();
    }
}
