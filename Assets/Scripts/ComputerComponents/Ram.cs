using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void OnDisable()
    {
        systemEventManager.OnAppOpen -= PushApp;
        systemEventManager.OnAppClosed -= RemoveApp;
    }
    private void PushApp(App app)
    {
        if (currentlyOpenApps.ContainsValue(app))
            return;

        currentlyOpenApps.Add(app.ID, app);
    }

    private void RemoveApp(string appID)
    {
        if (!currentlyOpenApps.ContainsKey(appID))
            return;

        currentlyOpenApps.Remove(appID);
    }


}
