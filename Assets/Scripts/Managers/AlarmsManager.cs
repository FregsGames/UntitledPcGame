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

    public List<AlarmData> Alarms { get => alarmList.Values.ToList(); }

    private Dictionary<string, (int,int)> soundedAlarms = new Dictionary<string, (int, int)>();

    private WaitForSeconds wait = new WaitForSeconds(5f);
    private bool AtLeastOneEnabledAlarm => alarmList.Count > 0 && alarmList.Any(a => a.Value.enabled);

    private bool checkingAlarms;

    private void OnEnable()
    {
        OnAlarmCreated += AddAlarm;
        OnAlarmRemoved += RemoveAlarm;
        eventManager.OnRestart += ClearAlarms;
    }

    private void ClearAlarms()
    {
        alarmList.Clear();
    }

    private void RemoveAlarm(AlarmData alarm)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.Cancel);
        alarmList.Remove(alarm.id);

        SaveAlarms();
    }

    private void AddAlarm(AlarmData alarm)
    {
        if (alarmList.ContainsKey(alarm.id))
        {
            alarmList[alarm.id] = alarm;
            if (soundedAlarms.ContainsKey(alarm.id))
            {
                soundedAlarms.Remove(alarm.id);
            }
        }
        else
        {
            alarmList.Add(alarm.id, alarm);
            SoundManager.Instance.PlaySound(SoundManager.Sound.AppOpen);
        }

        if(AtLeastOneEnabledAlarm && !checkingAlarms)
        { 
            StartCoroutine(AlarmCheck());
        }

        SaveAlarms();
    }

    private void OnDisable()
    {
        OnAlarmRemoved -= RemoveAlarm;
        OnAlarmCreated -= AddAlarm;
        eventManager.OnRestart -= ClearAlarms;
    }

    public void Initialize()
    {
        Dictionary<string, string> dictionary = SaveManager.Instance.RetrieveStringThatContains("alarm_");
        List<string> keys =  dictionary.Where(e => e.Key.Contains("alarm_key")).Select(x => x.Value).ToList();

        foreach (var key in keys)
        {
            AlarmData alarmData = new AlarmData()
            {
                id = key,
                description = dictionary[$"alarm_{key}_description"],
                enabled = dictionary[$"alarm_{key}_enabled"].Equals("true")? true : false,
                time = (int.Parse(dictionary[$"alarm_{key}_hour"]), int.Parse(dictionary[$"alarm_{key}_minute"]))
            };

            alarmList.Add(key, alarmData);
        }

        if (alarmList.Count > 0)
        {
            StartCoroutine(AlarmCheck());
        }
    }

    private void SaveAlarms()
    {
        foreach (var alarm in alarmList)
        {
            Dictionary<string, string> alarmInfo = new Dictionary<string, string>();
            alarmInfo.Add($"alarm_key_{alarm.Key}", alarm.Key);
            alarmInfo.Add($"alarm_{alarm.Key}_enabled", alarm.Value.enabled? "true" : "false");
            alarmInfo.Add($"alarm_{alarm.Key}_hour", alarm.Value.time.Item1.ToString());
            alarmInfo.Add($"alarm_{alarm.Key}_minute", alarm.Value.time.Item2.ToString());
            alarmInfo.Add($"alarm_{alarm.Key}_description", alarm.Value.description);
            SaveManager.Instance.Save(alarmInfo, alarm.Key);
        }
    }


    IEnumerator AlarmCheck()
    {
        checkingAlarms = true;

        while (AtLeastOneEnabledAlarm)
        {
            foreach (var alarm in alarmList.Values)
            {
                if(alarm.time.Item1 == DateTime.Now.Hour && alarm.time.Item2 == DateTime.Now.Minute)
                {
                    if (soundedAlarms.ContainsKey(alarm.id))
                    {
                        if (soundedAlarms[alarm.id].Item1 == DateTime.Now.Hour && 
                            soundedAlarms[alarm.id].Item2 == DateTime.Now.Minute)
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

        checkingAlarms = false;
    }
}
