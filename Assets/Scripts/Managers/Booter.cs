using System.Threading.Tasks;
using UnityEngine;

public class Booter : MonoBehaviour
{
    [SerializeField]
    private BootScreen bootScreen;

    [SerializeField]
    private bool showBootScreen = true;

    private async Task Start()
    {
        if (showBootScreen)
            await bootScreen.ShowBootScreen();

        SaveManager.Instance.LoadSave();
        Computer.Instance.Desktop.LoadState();
        Computer.Instance.NotificationCenter.Initialize();

        await Task.Delay(1000);

        FolderManager.Instance.Initialize();
        AlarmsManager.Instance.LoadSettings();

        await bootScreen.HideBootScreen();
    }
}
