using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct AlarmData
{
    public string id;
    public string description;
    public (int, int) time;
    public bool enabled;
}

public class Alarm : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private TextMeshProUGUI desc;
    [SerializeField]
    private SystemEventManager eventManager;

    public AlarmData AlarmData { get => alarmData; set => alarmData = value; }
    private AlarmData alarmData;

    public bool AlarmEnabled { get; set; }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(SetAlarmEnabled);
        
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    private void SetAlarmEnabled(bool state)
    {
        AlarmEnabled = state;
        alarmData.enabled = state;
        AlarmsManager.Instance.OnAlarmCreated?.Invoke(alarmData);
    }

    public void RemoveAlarm()
    {
        AlarmsManager.Instance.OnAlarmRemoved?.Invoke(AlarmData);
        Destroy(gameObject);
    }

    public void Setup(AlarmData alarmData, bool notify)
    {
        AlarmData = alarmData;

        this.alarmData.id = string.IsNullOrEmpty(alarmData.id) ? Guid.NewGuid().ToString() : alarmData.id;
        SetFormatedTime(alarmData.time);
        desc.text = alarmData.description;
        toggle.SetIsOnWithoutNotify(alarmData.enabled);
        if (notify)
        {
            AlarmsManager.Instance.OnAlarmCreated?.Invoke(AlarmData);
        }
    }

    private void SetFormatedTime((int, int) time)
    {
        string hourFormatted = (time.Item1 < 10) ? $"0{time.Item1}" : $"{time.Item1}";
        string minutesFormatted = (time.Item2 < 10) ? $"0{time.Item2}" : $"{time.Item2}";

        text.text = $"{hourFormatted}:{minutesFormatted}";
    }

    public void ModifyAlarm()
    {
        ((NumericPopup)eventManager.RequestPopUp("Introduce la hora (hh:mm)", App.AppType.NumericPopup)).Setup(4);
        eventManager.OnPopUpCancel += UnsubscribeNumericPopup;
        eventManager.OnNumericPopUpNumberSubmit += SetAlarmTime;
    }

    public void ModifyDescription()
    {
        eventManager.RequestPopUp("Descripción", App.AppType.StringPopup);
        eventManager.OnPopUpCancel += UnsubscribeStringPopup;
        eventManager.OnStringPopUpSubmit += SetDescription;
    }

    private void SetDescription(string newDesc)
    {
        alarmData.description = newDesc;
        desc.text = newDesc;
        UnsubscribeStringPopup();
        AlarmsManager.Instance.OnAlarmCreated?.Invoke(alarmData);
    }

    private void UnsubscribeStringPopup()
    {
        eventManager.OnPopUpCancel -= UnsubscribeNumericPopup;
    }

    private void SetAlarmTime(string time)
    {
        int hour = int.Parse(time.Substring(0, 2));
        int minutes = int.Parse(time.Substring(2, 2));

        if (hour > 23 || minutes > 59)
            return;

        alarmData.time = (hour, minutes);
        SetFormatedTime(alarmData.time);
        AlarmsManager.Instance.OnAlarmCreated?.Invoke(alarmData);
        UnsubscribeNumericPopup();
    }

    private void UnsubscribeNumericPopup()
    {
        eventManager.OnPopUpCancel -= UnsubscribeNumericPopup;
        eventManager.OnNumericPopUpNumberSubmit -= SetAlarmTime;
    }
}
