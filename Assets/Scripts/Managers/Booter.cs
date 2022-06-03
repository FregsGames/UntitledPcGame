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

        Computer.Instance.Desktop.LoadState();
        Computer.Instance.NotificationCenter.Initialize();
        FolderManager.Instance.Initialize();
        Computer.Instance.NotificationCenter.RequestNotification(null, "Buenos días", "Todo listo", null);

        await bootScreen.HideBootScreen();
    }
}
