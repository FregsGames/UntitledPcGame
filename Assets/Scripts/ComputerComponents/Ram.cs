using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static App;

public class Ram : SerializedMonoBehaviour
{
    [SerializeField]
    protected SystemEventManager systemEventManager;

    [SerializeField]
    private Dictionary<string, App> currentlyOpenApps = new Dictionary<string, App>();

    private void OnEnable()
    {
        systemEventManager.OnAppOpen += PushApp;
        systemEventManager.OnAppClosed += RemoveApp;
        systemEventManager.OnPopUpCancel = () => { SoundManager.Instance.PlaySound(SoundManager.Sound.Cancel); };
    }

    private void OnDisable()
    {
        systemEventManager.OnAppOpen -= PushApp;
        systemEventManager.OnAppClosed -= RemoveApp;
        systemEventManager.OnPopUpCancel = null;
    }

    public bool IsAppOpen(AppType appType)
    {
        return currentlyOpenApps.Values.FirstOrDefault(app => app.Type == appType) != null;
    }

    private void PushApp(App app)
    {
        if (currentlyOpenApps.ContainsValue(app))
            return;

        if (app is not IPopUp)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.AppOpen, 1 + currentlyOpenApps.Count * 0.025f);
        }

        currentlyOpenApps.Add(app.ID, app);
    }

    private void RemoveApp(string appID)
    {
        if (!currentlyOpenApps.ContainsKey(appID))
            return;

        if (currentlyOpenApps[appID] is not IPopUp)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Cancel);
        }

        currentlyOpenApps.Remove(appID);
    }


}
