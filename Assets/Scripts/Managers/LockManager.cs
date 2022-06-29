using System;
using System.Collections.Generic;
using UnityEngine;
using static LockManager;

public class LockManager : SerializedSingleton<LockManager>
{
    public enum LockType { NumericPass }
    [SerializeField]
    private Dictionary<App.AppType, Lock> lockedApps = new Dictionary<App.AppType, Lock>();
    [SerializeField]
    private Dictionary<string, Lock> lockedFiles = new Dictionary<string, Lock>();


    [SerializeField]
    private SystemEventManager systemEventManager;

    private App.AppType currentApp;
    private string currentFile;
    private bool unlockingApp = true;

    public void LoadSettings()
    {
        LoadLockedApps();
        LoadLockedFiles();
    }

    private void LoadLockedFiles()
    {
        Dictionary<string, string> lockedDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_lockedFile");
        Dictionary<string, string> lockedTypeDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_lockTypeFile");
        Dictionary<string, string> lockedPassDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_lockPassFile");

        if (lockedDictionary.Count > 0)
        {
            lockedFiles.Clear();
        }
        else
        {
            return;
        }

        foreach (var lockitem in lockedDictionary)
        {
            var id = lockitem.Key.Split("_")[0];
            lockedFiles.Add(id, null);

            if (lockedTypeDictionary.ContainsKey($"{id}_lockTypeFile"))
            {
                if (lockedTypeDictionary[$"{id}_lockTypeFile"] == "NumericPass")
                {
                    lockedFiles[id] = new NumericPassLock() { lockType = LockType.NumericPass };
                    ((NumericPassLock)lockedFiles[id]).password = lockedPassDictionary[$"{id}_lockPassFile"];
                }
                else
                {
                    // TODO
                }
                lockedFiles[id].isLocked = lockitem.Value == "true";
            }
        }
    }

    private void LoadLockedApps()
    {
        Dictionary<string, string> lockedDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_locked");
        Dictionary<string, string> lockedTypeDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_lockType");
        Dictionary<string, string> lockedPassDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_lockPass");

        if (lockedDictionary.Count > 0)
        {
            lockedApps.Clear();
        }
        else
        {
            return;
        }

        foreach (var lockitem in lockedDictionary)
        {
            var enumName = lockitem.Key.Split("_")[0];
            lockedApps.Add(GetAppEnum(enumName), null);

            if (lockedTypeDictionary.ContainsKey($"{enumName}_lockType"))
            {
                if (lockedTypeDictionary[$"{enumName}_lockType"] == "NumericPass")
                {
                    lockedApps[GetAppEnum(enumName)] = new NumericPassLock() {lockType = LockType.NumericPass};
                    ((NumericPassLock)lockedApps[GetAppEnum(enumName)]).password = lockedPassDictionary[$"{enumName}_lockPass"];
                }
                else
                {
                    // TODO
                }
                lockedApps[GetAppEnum(enumName)].isLocked = lockitem.Value == "true";
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

        foreach (var file in lockedFiles)
        {
            SaveManager.Instance.Save($"{file.Key}_lockedFile", file.Value.isLocked ? "true" : "false");
            SaveManager.Instance.Save($"{file.Key}_lockTypeFile", file.Value.lockType.ToString());

            if (file.Value is NumericPassLock)
            {
                SaveManager.Instance.Save($"{file.Key}_lockPassFile", ((NumericPassLock)file.Value).password);
            }
        }
    }

    public bool IsLocked(App.AppType app)
    {
        if (app == App.AppType.Folder || app == App.AppType.TextFile)
            return false;

        return lockedApps.ContainsKey(app) && lockedApps[app].isLocked;
    }

    public bool IsLocked(string id)
    {
        return lockedFiles.ContainsKey(id) && lockedFiles[id].isLocked;
    }

    public void ResolveOpenAttempt(App.AppType app)
    {
        switch (lockedApps[app].lockType)
        {
            case LockType.NumericPass:
                unlockingApp = true;
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

    public void ResolveOpenAttempt(string id)
    {
        switch (lockedFiles[id].lockType)
        {
            case LockType.NumericPass:
                unlockingApp = false;
                currentFile = id;
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

        if (unlockingApp)
        {
            TryToUnlock(currentApp, input);
        }
        else
        {
            TryToUnlock(currentFile, input);
        }
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

    private bool TryToUnlock(string id, string password)
    {
        if (!lockedFiles.ContainsKey(id))
            return false;

        Lock lockedApp = lockedFiles[id];

        if (!lockedApp.isLocked)
            return true;

        if (lockedApp.lockType != LockType.NumericPass)
            return false;

        if (((NumericPassLock)lockedApp).password == password)
        {
            lockedApp.isLocked = false;
            systemEventManager.OnFileUnlocked?.Invoke(id);
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



