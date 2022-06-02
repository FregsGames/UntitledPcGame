using UnityEngine;

public class Booter : MonoBehaviour
{
    private void Start()
    {
        // TODO: AQUÍ METER UNA PANTALLA DE CARGA

        Computer.Instance.Desktop.LoadState();
        Computer.Instance.NotificationCenter.Initialize();
        FolderManager.Instance.Initialize();
        Computer.Instance.NotificationCenter.RequestNotification(null, "Buenos días", "Todo listo", null);

    }
}
