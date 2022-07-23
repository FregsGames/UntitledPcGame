using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "AppsRequirements", menuName = "ScriptableObjects/AppsRequirements", order = 5)]
public class AppsRequirenments : SerializedScriptableObject
{
    [SerializeField]
    private Dictionary<App.AppType, AppRequirement> appRequirements = new Dictionary<App.AppType, AppRequirement>();

    public bool RequiresWifi(App.AppType app)
    {
        if (!appRequirements.ContainsKey(app))
            return false;

        return appRequirements[app].requiresWifi;
    }

    public bool RequiresAdmin(App.AppType app)
    {
        if (!appRequirements.ContainsKey(app))
            return false;

        return appRequirements[app].requiresAdmin;
    }
}

[Serializable]
public class AppRequirement
{
    public bool requiresWifi;
    public bool requiresAdmin;
}
