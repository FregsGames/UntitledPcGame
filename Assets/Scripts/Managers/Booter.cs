using UnityEngine;

public class Booter : MonoBehaviour
{
    private void Start()
    {
        // TODO: AQUÍ METER UNA PANTALLA DE CARGA

        Computer.Instance.Desktop.LoadState();
    }
}
