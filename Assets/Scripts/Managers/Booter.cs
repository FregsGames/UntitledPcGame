using UnityEngine;

public class Booter : MonoBehaviour
{
    private void Start()
    {
        // TODO: AQU� METER UNA PANTALLA DE CARGA

        Computer.Instance.Desktop.LoadState();
        Computer.Instance.NotificationCenter.Initialize();
        FolderManager.Instance.Initialize();
        Computer.Instance.NotificationCenter.RequestNotification(null, "Buenos d�as", "Todo listo", null);

    }
}
