using UnityEngine;

public class Booter : MonoBehaviour
{
    private void Start()
    {
        // TODO: AQU� METER UNA PANTALLA DE CARGA

        Computer.Instance.Desktop.LoadState();
    }
}
