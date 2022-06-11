using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AlarmsManager : Singleton<AlarmsManager>
{
    [SerializeField]
    private SystemEventManager eventManager;

    public Action<AlarmData> OnAlarmCreated;
    public Action<AlarmData> OnAlarmRemoved;

    private Dictionary<string, AlarmData> alarmList = new Dictionary<string, AlarmData>();

    private Dictionary<string, (int,int)> soundedAlarms = new Dictionary<string, (int, int)>();

    private WaitForSeconds wait = new WaitForSeconds(5f);

    private void OnEnable()
    {
        OnAlarmCreated += AddAlarm;
        OnAlarmRemoved += RemoveAlarm;
    }

    private void RemoveAlarm(AlarmData alarm)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.Cancel);
        alarmList.Remove(alarm.id);
    }

    private void AddAlarm(AlarmData alarm)
    {
        if (alarmList.ContainsKey(alarm.id))
        {
            alarmList[alarm.id] = alarm;
            return;
        }

        alarmList.Add(alarm.id, alarm);
        SoundManager.Instance.PlaySound(SoundManager.Sound.AppOpen);

        if(alarmList.Count == 1)
        {
            StartCoroutine(AlarmCheck());
        }
    }

    private void OnDisable()
    {
        OnAlarmRemoved -= RemoveAlarm;
        OnAlarmCreated -= AddAlarm;
    }

    public void LoadSettings()
    {

    }

    IEnumerator AlarmCheck()
    {
        while(alarmList.Count > 0)
        {
            foreach (var alarm in alarmList.Values)
            {
                if(alarm.time.Item1 == DateTime.Now.Hour && alarm.time.Item2 == DateTime.Now.Minute)
                {
                    if (soundedAlarms.ContainsKey(alarm.id))
                    {
                        if (soundedAlarms[alarm.id].Item1 == DateTime.Now.Hour && soundedAlarms[alarm.id].Item2 == DateTime.Now.Minute)
                        {
                            continue;
                        }
                        else
                        {
                            soundedAlarms.Remove(alarm.id);
                        }
                    } 

                    Computer.Instance.NotificationCenter.RequestNotification(null, "Alarma", alarm.description, null);
                    soundedAlarms.Add(alarm.id, alarm.time);
                }
            }
            yield return wait;
        }
    }
}
