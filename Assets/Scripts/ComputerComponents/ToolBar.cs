using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : MonoBehaviour
{
    [SerializeField]
    private SystemEventManager eventManager;

    [SerializeField]
    private Image wifiCancelIcon;

    private void OnEnable()
    {
        eventManager.OnWifiEnabled += EnableIcon;

    }

    private void EnableIcon(bool state)
    {
        wifiCancelIcon.gameObject.SetActive(!state);
    }

    private void OnDisable()
    {
        eventManager.OnWifiEnabled -= EnableIcon;
    }

    public async void Save()
    {
        ComputerScreen.Instance.BlockPanel.EnableBlock(true);
        await SaveManager.Instance.SaveChanges();
        await Task.Delay(1000);
        ComputerScreen.Instance.BlockPanel.EnableBlock(false);
    }

    public void OpenSettings()
    {
        InstantiatorManager.Instance.Instantiate(App.AppType.Settings);
    }
}
