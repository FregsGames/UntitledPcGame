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

public class Alarm : MonoBehaviour, IPointerClickHandler
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
        toggle.onValueChanged.AddListener(SetAlamrEnabled);
    }

    private void SetAlamrEnabled(bool state)
    {
        AlarmEnabled = state;
    }

    public void RemoveAlarm()
    {
        AlarmsManager.Instance.OnAlarmRemoved?.Invoke(AlarmData);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    public void Setup(AlarmData alarmData)
    {
        AlarmData = alarmData;

        this.alarmData.id = string.IsNullOrEmpty(alarmData.id) ? Guid.NewGuid().ToString() : alarmData.id;
        SetFormatedTime(alarmData.time);
        desc.text = alarmData.description;
        toggle.SetIsOnWithoutNotify(enabled);
        AlarmsManager.Instance.OnAlarmCreated?.Invoke(alarmData);
    }

    private void SetFormatedTime((int, int) time)
    {
        string hourFormatted = (time.Item1 < 10) ? $"0{time.Item1}" : $"{time.Item1}";
        string minutesFormatted = (time.Item2 < 10) ? $"0{time.Item2}" : $"{time.Item2}";

        text.text = $"{hourFormatted}:{minutesFormatted}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ((NumericPopup)eventManager.RequestPopUp("Introduce la hora (hh:mm)", App.AppType.NumericPopup)).Setup(4);
        eventManager.OnPopUpCancel += UnsubscribePopup;
        eventManager.OnNumericPopUpNumberSubmit += SetAlarmTime;
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
    }

    private void UnsubscribePopup()
    {

    }
}
