using System;
using UnityEngine;
using UnityEngine.UI;

public class AlarmsApp : App
{
    [SerializeField]
    private GameObject alarmPrefab;
    [SerializeField]
    private Button addAlarmButton;
    [SerializeField]
    private Transform contanier;

    [SerializeField]
    private int maxAlarmCount = 10;

    private void OnEnable()
    {
        addAlarmButton.onClick.AddListener(CreateNewAlarm);
        InstantiateAlarms();
    }

    private void OnDisable()
    {
        addAlarmButton.onClick.RemoveAllListeners();
    }

    private void InstantiateAlarms()
    {
        foreach (var alarmData in AlarmsManager.Instance.Alarms)
        {
            var alarmGO = Instantiate(alarmPrefab, contanier);
            alarmGO.GetComponent<Alarm>().Setup(alarmData, false);
        }
    }

    private void CreateNewAlarm()
    {
        int alamarCount = contanier.GetComponentsInChildren<Alarm>().Length;
        if (alamarCount == maxAlarmCount)
        {
            systemEventManager.RequestPopUp("Tienes demasiadas alarmas", AppType.ConfirmationPopup);
            return;
        }

        Alarm alarm = Instantiate(alarmPrefab, contanier).GetComponent<Alarm>();
        AlarmData alarmData = new AlarmData()
        {
            time = (00, 00),
            description = "Esto es una alarma.",
            enabled = true
        };
        alarm.Setup(alarmData, true);
    }


}
