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
    }
    private void OnDisable()
    {
        addAlarmButton.onClick.RemoveAllListeners();
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
            time = (18, 07),
            description = "Esto es una alarma.",
            enabled = true
        };
        alarm.Setup(alarmData);
    }


}
