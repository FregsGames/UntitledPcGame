using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FirstGameLoader : SerializedSingleton<FirstGameLoader>
{
    [SerializeField]
    private GameObject iconPrefab;
    [Header("Debug")]
    [SerializeField]
    private (int,int) firstAlarmTime;

    public void LoadFirstGameData()
    {
        CreateFirstAlarm();
        CreateLockForSecurityCameras();
        LoadDesktopIcons();
        LockManager.instance.AddLock("wifi", false, "1234");
    }
    private void CreateFirstAlarm()
    {
        var alarmTime = DateTime.Now.AddMinutes(1);
        firstAlarmTime = (alarmTime.Hour, alarmTime.Minute);

        AlarmData alarmData = new AlarmData()
        {
            description = "Alarma con información importante. NO BORRAR",
            enabled = true,
            id = Guid.NewGuid().ToString(),
            time = firstAlarmTime
        };
        AlarmsManager.Instance.OnAlarmCreated?.Invoke(alarmData);
    }

    private void CreateLockForSecurityCameras()
    {
        LockManager.Instance.ClearAll();

        LockManager.Instance.AddLock(App.AppType.SecurityCameras, firstAlarmTime.Item1.ToString() + firstAlarmTime.Item2.ToString());
        FindObjectsOfType<Icon>().FirstOrDefault(i => i.AssociatedAppType == App.AppType.SecurityCameras).SetLock(true);
    }

    private void LoadDesktopIcons()
    {
        Computer.Instance.Desktop.RemoveChildren();
        var cameraIcon = Instantiate(iconPrefab, Computer.Instance.Desktop.transform).GetComponent<Icon>();
        cameraIcon.Setup(App.AppType.SecurityCameras, "Security Cameras", "camera", true);
        Computer.Instance.Desktop.MoveIconTo(cameraIcon, Vector2.zero);
    }
}
