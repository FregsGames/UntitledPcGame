using UnityEngine;

public class Initializator : MonoBehaviour
{
    private void Start()
    {
        // TODO: AQU� METER UNA PANTALLA DE CARGA

        Computer.Instance.FileSystem.MainDisk.Initialize();
    }
}
