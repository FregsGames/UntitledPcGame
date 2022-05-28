using System;
using UnityEngine;

[CreateAssetMenu(fileName = "System Event Manager", menuName = "ScriptableObjects/System Event Manager", order = 4)]
public class SystemEventManager : ScriptableObject
{
    public Action<App> OnAppOpen;
    public Action<string> OnAppClosed;
    public Action OnLanguagueLoaded;
    
    public Action<bool> OnPopUpSubmit;



    public bool RequestPopUp()
    {
        if (Computer.Instance.Ram.IsAppOpen(App.AppType.ConfirmationPopup))
        {
            return false;
        }

        InstantiatorManager.Instance.Instantiate(App.AppType.ConfirmationPopup);
        return true;
    }
}
