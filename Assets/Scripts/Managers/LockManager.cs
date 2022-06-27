using System;
using System.Collections.Generic;
using UnityEngine;
using static LockManager;

public class LockManager : SerializedSingleton<LockManager>
{
    public enum LockType { NumericPass }
    [SerializeField]
    private Dictionary<App.AppType, Lock> lockedApps = new Dictionary<App.AppType, Lock>();
    public Dictionary<App.AppType, Lock> LockedApps { get => lockedApps; set => lockedApps = value; }
    [SerializeField]
    private SystemEventManager systemEventManager;

    private App.AppType currentApp;

    public void LoadSettings()
    {
        Dictionary<string, string> lockedDictionary = SaveManager.Instance.RetrieveStringThatContains("_locked");

        if(lockedDictionary.Count > 0)
        {
            lockedApps.Clear();
        }
        else
        {
            return;
        }

        foreach (var lockitem in SaveManager.Instance.RetrieveStringThatContains("_locked"))
        {
            var enumName = lockitem.Key.Split("_")[0];
            lockedApps.Add(GetAppEnum(enumName), new Lock() {isLocked = lockitem.Value == "true"});
        }

        foreach (var lockType in SaveManager.Instance.RetrieveStringThatContains("_lockType"))
        {
            var enumName = lockType.Key.Split("_")[0];
            lockedApps[GetAppEnum(enumName)].lockType = GetLockEnum(lockType.Value);
        }

        foreach (var lockPass in SaveManager.Instance.RetrieveStringThatContains("_lockPass"))
        {
            var enumName = lockPass.Key.Split("_")[0];
            Lock currentLock = lockedApps[GetAppEnum(enumName)];
            if(currentLock is NumericPassLock)
            {
                ((NumericPassLock)currentLock).password = lockPass.Value;
            }
        }
    }
    private void SaveChanges()
    {
        foreach (var app in lockedApps)
        {
            SaveManager.Instance.Save($"{app.Key}_locked", app.Value.isLocked? "true" : "false");
            SaveManager.Instance.Save($"{app.Key}_lockType", app.Value.lockType.ToString());

            if(app.Value is NumericPassLock)
            {
                SaveManager.Instance.Save($"{app.Key}_lockPass", ((NumericPassLock)app.Value).password);
            }
        }
    }

    public bool IsLocked(App.AppType app)
    {
        return lockedApps.ContainsKey(app) && lockedApps[app].isLocked;
    }

    public void ResolveOpenAttempt(App.AppType app)
    {
        switch (lockedApps[app].lockType)
        {
            case LockType.NumericPass:
                currentApp = app;
                systemEventManager.OnNumericPopUpNumberSubmit += OnNumericPassReceived;
                systemEventManager.OnPopUpCancel += OnPopUpCancel;
                ((NumericPopup)systemEventManager.RequestPopUp("Introduce la contraseña", App.AppType.NumericPopup)).Setup(4);
                break;
            default:
                systemEventManager.RequestPopUp("Esta app está bloqueada", App.AppType.ConfirmationPopup);
                break;
        }

    }

    private void OnPopUpCancel()
    {
        systemEventManager.OnNumericPopUpNumberSubmit -= OnNumericPassReceived;
        systemEventManager.OnPopUpCancel -= OnPopUpCancel;
    }

    private void OnNumericPassReceived(string input)
    {
        systemEventManager.OnNumericPopUpNumberSubmit -= OnNumericPassReceived;
        systemEventManager.OnPopUpCancel -= OnPopUpCancel;

        TryToUnlock(currentApp, input);
    }


    private bool TryToUnlock(App.AppType app, string password)
    {
        if (!lockedApps.ContainsKey(app))
            return false;

        Lock lockedApp = lockedApps[app];

        if (!lockedApp.isLocked)
            return true;

        if (lockedApp.lockType != LockType.NumericPass)
            return false;

        if(((NumericPassLock) lockedApp).password == password)
        {
            lockedApp.isLocked = false;
            systemEventManager.OnAppUnlocked?.Invoke(app);
            SaveChanges();
            return true;
        }

        return false;
    }

    private static App.AppType GetAppEnum(string enumName)
    {
        return (App.AppType)Enum.Parse(typeof(App.AppType), enumName);
    }

    private static LockType GetLockEnum(string enumName)
    {
        return (LockType)Enum.Parse(typeof(LockType), enumName);
    }

    public class Lock
    {
        public LockType lockType;
        public bool isLocked;
    }
    public class NumericPassLock : Lock
    {
        public string password;
    }
}



