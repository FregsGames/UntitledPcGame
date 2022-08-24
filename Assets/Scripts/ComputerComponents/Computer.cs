using Assets.Scripts.Apps;
using System;
using UnityEngine;

public class Computer : Singleton<Computer>
{
    [SerializeField]
    private Ram ram;
    [SerializeField]
    private ComputerSettings computerSettings;
    [SerializeField]
    private Desktop desktop;
    [SerializeField]
    private NotificationCenter notificationCenter;
    [SerializeField]
    private SystemEventManager eventManager;
    [SerializeField]
    private ToolBar toolbar;
    [SerializeField]
    private Booter booter;


    public ComputerSettings ComputerSettings { get => computerSettings; set => computerSettings = value; }
    public Ram Ram { get => ram; set => ram = value; }
    public Desktop Desktop { get => desktop; set => desktop = value; }
    public NotificationCenter NotificationCenter { get => notificationCenter; set => notificationCenter = value; }

    public void ShowTurnOffPopUp()
    {
        eventManager.RequestPopUp("¿Apagar el equipo?",App.AppType.ConfirmationPopup);
        eventManager.OnPopUpSubmit += TurnOff;
    }

    private void TurnOff(bool state)
    {
        eventManager.OnPopUpSubmit -= TurnOff;

        if (state)
        {
            Application.Quit();
        }
    }

    public async void Restart()
    {
        eventManager.OnRestart?.Invoke();
        await toolbar.Save();
        await booter.Start();
    }

}
