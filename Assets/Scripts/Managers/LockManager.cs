using System;
using System.Collections.Generic;
using UnityEngine;
using static LockManager;
using System.Linq;

public class LockManager : SerializedSingleton<LockManager>
{
    public enum LockType { NumericPass }
    public enum CurrentlyUnlocking { none, app, file, other }


    [SerializeField]
    private Dictionary<App.AppType, Lock> lockedApps = new Dictionary<App.AppType, Lock>();
    [SerializeField]
    private Dictionary<string, Lock> lockedFiles = new Dictionary<string, Lock>();
    [SerializeField]
    private Dictionary<string, Lock> otherLocks = new Dictionary<string, Lock>();


    [SerializeField]
    private SystemEventManager systemEventManager;

    private App.AppType currentUnlockingApp;
    private string currentUnlockingId;
    private CurrentlyUnlocking currentlyUnlocking = CurrentlyUnlocking.none;

    public void LoadSettings()
    {
        if (!SaveManager.Instance.SaveDataFound)
        {
            foreach (var icon in FindObjectsOfType<Icon>(true))
            {
                icon.SetLock(lockedApps.Any(l => l.Key == icon.AssociatedAppType && l.Value.isLocked) || lockedFiles.Any(l => l.Key == icon.AssociatedAppID && l.Value.isLocked));
            }
        }
        else
        {
            ClearAll();
            LoadLockedFiles();
            LoadLockedApps();
        }
    }

    public void ClearAll()
    {
        lockedApps.Clear();
        lockedFiles.Clear();
        otherLocks.Clear();
    }

    private void LoadLockedFiles()
    {
        Dictionary<string, string> lockedDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_lockedFile");
        Dictionary<string, string> lockedTypeDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_lockTypeFile");
        Dictionary<string, string> lockedPassDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_lockPassFile");


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

    private void LoadOtherLocks()
    {
        Dictionary<string, string> lockedDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_otherLock");
        Dictionary<string, string> lockedTypeDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_otherTypeLock");
        Dictionary<string, string> lockedPassDictionary = SaveManager.Instance.RetrieveStringThatEndsWith("_otherPassLock");


        foreach (var lockitem in lockedDictionary)
        {
            var id = lockitem.Key.Split("_")[0];
            otherLocks.Add(id, null);

            if (lockedTypeDictionary.ContainsKey($"{id}_otherTypeLock"))
            {
                if (lockedTypeDictionary[$"{id}_lockTypeFile"] == "NumericPass")
                {
                    otherLocks[id] = new NumericPassLock() { lockType = LockType.NumericPass };
                    ((NumericPassLock)otherLocks[id]).password = lockedPassDictionary[$"{id}_lockPassFile"];
                }
                else
                {
                    // TODO
                }
                otherLocks[id].isLocked = lockitem.Value == "true";
            }
        }
    }

    public void AddLock(App.AppType appType, string password)
    {
        if (lockedApps.ContainsKey(appType))
        {
            lockedApps[appType] = new NumericPassLock() {isLocked = true, lockType = LockType.NumericPass, password = password };
        }
        else
        {
            lockedApps.Add(appType, new NumericPassLock() { isLocked = true, lockType = LockType.NumericPass, password = password });
        }
        SaveChanges();
    }

    public void AddLock(string id, bool isFile, string password)
    {
        if (isFile)
        {
            if (lockedFiles.ContainsKey(id))
            {
                lockedFiles[id] = new NumericPassLock() { isLocked = true, lockType = LockType.NumericPass, password = password };
            }
            else
            {
                lockedFiles.Add(id, new NumericPassLock() { isLocked = true, lockType = LockType.NumericPass, password = password });
            }
        }
        else
        {
            if (otherLocks.ContainsKey(id))
            {
                otherLocks[id] = new NumericPassLock() { isLocked = true, lockType = LockType.NumericPass, password = password };
            }
            else
            {
                otherLocks.Add(id, new NumericPassLock() { isLocked = true, lockType = LockType.NumericPass, password = password });
            }
        }

        SaveChanges();
    }

    private void SaveChanges()
    {
        SaveManager.Instance.RemoveEntriesThatContains("_locked");
        SaveManager.Instance.RemoveEntriesThatContains("_lockType");
        SaveManager.Instance.RemoveEntriesThatContains("_lockPass");
        SaveManager.Instance.RemoveEntriesThatContains("_lockedFile");
        SaveManager.Instance.RemoveEntriesThatContains("_lockTypeFile");
        SaveManager.Instance.RemoveEntriesThatContains("_lockPassFile");

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

        foreach (var other in otherLocks)
        {
            SaveManager.Instance.Save($"{other.Key}_otherLock", other.Value.isLocked ? "true" : "false");
            SaveManager.Instance.Save($"{other.Key}_otherTypeLock", other.Value.lockType.ToString());

            if (other.Value is NumericPassLock)
            {
                SaveManager.Instance.Save($"{other.Key}_otherPassLock", ((NumericPassLock)other.Value).password);
            }
        }
    }

    public bool IsLocked(App.AppType app)
    {
        if (app == App.AppType.Folder || app == App.AppType.TextFile)
            return false;

        return lockedApps.ContainsKey(app) && lockedApps[app].isLocked;
    }

    public bool IsLocked(string id, bool isFile)
    {
        if (isFile)
        {
            return lockedFiles.ContainsKey(id) && lockedFiles[id].isLocked;
        }
        else
        {
            return otherLocks.ContainsKey(id) && otherLocks[id].isLocked;
        }
    }

    public void ResolveOpenAttempt(App.AppType app)
    {
        switch (lockedApps[app].lockType)
        {
            case LockType.NumericPass:
                currentlyUnlocking = CurrentlyUnlocking.app;
                currentUnlockingApp = app;

                systemEventManager.OnNumericPopUpNumberSubmit += OnNumericPassReceived;
                systemEventManager.OnPopUpCancel += OnPopUpCancel;
                ((NumericPopup)systemEventManager.RequestPopUp("Introduce la contraseña", App.AppType.NumericPopup)).
                    Setup(((NumericPassLock)lockedApps[app]).password.Length);
                break;
            default:
                systemEventManager.RequestPopUp("Esta app está bloqueada", App.AppType.ConfirmationPopup);
                break;
        }
    }

    public void ResolveOpenAttempt(string id, bool file)
    {
        if (file)
        {
            switch (lockedFiles[id].lockType)
            {
                case LockType.NumericPass:
                    currentlyUnlocking = CurrentlyUnlocking.file;
                    currentUnlockingId = id;
                    systemEventManager.OnNumericPopUpNumberSubmit += OnNumericPassReceived;
                    systemEventManager.OnPopUpCancel += OnPopUpCancel;
                    ((NumericPopup)systemEventManager.RequestPopUp("Introduce la contraseña", App.AppType.NumericPopup)).
                    Setup(((NumericPassLock)lockedFiles[id]).password.Length);
                    break;
                default:
                    systemEventManager.RequestPopUp("Esta app está bloqueada", App.AppType.ConfirmationPopup);
                    break;
            }
        }
        else
        {
            switch (otherLocks[id].lockType)
            {
                case LockType.NumericPass:
                    currentlyUnlocking = CurrentlyUnlocking.other;
                    currentUnlockingId = id;
                    systemEventManager.OnNumericPopUpNumberSubmit += OnNumericPassReceived;
                    systemEventManager.OnPopUpCancel += OnPopUpCancel;
                    ((NumericPopup)systemEventManager.RequestPopUp("Introduce la contraseña", App.AppType.NumericPopup)).
                    Setup(((NumericPassLock)otherLocks[id]).password.Length);
                    break;
                default:
                    break;
            }
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

        switch (currentlyUnlocking)
        {
            case CurrentlyUnlocking.none:
                break;
            case CurrentlyUnlocking.app:
                TryToUnlock(currentUnlockingApp, input);
                break;
            case CurrentlyUnlocking.file:
            case CurrentlyUnlocking.other:
                TryToUnlock(currentUnlockingId, input);
                break;
            default:
                break;
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
        Lock lockedApp;

        if (currentlyUnlocking == CurrentlyUnlocking.file)
        {
            if (!lockedFiles.ContainsKey(id))
                return false;

            lockedApp = lockedFiles[id];
        }
        else
        {
            if (!otherLocks.ContainsKey(id))
                return false;

            lockedApp = otherLocks[id];
        }

        if (lockedApp == null)
            return false;

        if (!lockedApp.isLocked)
            return true;

        if (lockedApp.lockType != LockType.NumericPass)
            return false;

        if (((NumericPassLock)lockedApp).password == password)
        {
            lockedApp.isLocked = false;
            if(currentlyUnlocking == CurrentlyUnlocking.file)
            {
                systemEventManager.OnFileUnlocked?.Invoke(id);
            }
            else
            {
                systemEventManager.OnOtherUnlocked?.Invoke(id);
            }
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



