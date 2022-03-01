using UnityEngine;

public class Initializator : MonoBehaviour
{
    private void Start()
    {
        // TODO: AQUÍ METER UNA PANTALLA DE CARGA

        Computer.Instance.FileSystem.MainDisk.Initialize();
    }
}
