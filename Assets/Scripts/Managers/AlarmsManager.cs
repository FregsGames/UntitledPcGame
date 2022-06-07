using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmsManager : Singleton<AlarmsManager>
{
    public Action<Alarm> OnAlarmCreated;
    public Action<Alarm> OnAlarmRemoved;

    private void OnEnable()
    {
        OnAlarmCreated += AddAlarm;
    }

    private void AddAlarm(Alarm alarm)
    {

    }

    private void OnDisable()
    {
        OnAlarmCreated -= AddAlarm;
    }

    public void LoadSettings()
    {

    }
}
