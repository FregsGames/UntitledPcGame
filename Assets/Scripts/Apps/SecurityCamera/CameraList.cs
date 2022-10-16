using UnityEngine;
using TMPro;

public class CameraList : MonoBehaviour
{
    [SerializeField]
    private SystemEventManager eventManager;
    [SerializeField]
    private CameraButton buttonPrefab;
    [SerializeField]
    private TextMeshProUGUI noConnectionText;
    [SerializeField]
    private TextMeshProUGUI noCamerasText;

    private void OnEnable()
    {
        eventManager.OnWifiEnabled += ShowCameras;
        ShowCameras(Computer.Instance.ComputerSettings.WifiEnabled);
    }

    private void OnDisable()
    {
        eventManager.OnWifiEnabled -= ShowCameras;
    }

    private void ShowCameras(bool showCameras)
    {
        MyUtils.Instance.DestroyChildren(transform, typeof(CameraButton));

        if (showCameras)
        {
            noConnectionText.enabled = false;

            int cameraCount = 0;

            foreach (var camera in SecurityCameraManager.Instance.SecurityCameras)
            {
                if(camera.Value.state != SecurityCameraManager.CameraVisibility.Locked)
                {
                    cameraCount++;
                    CameraButton cameraButton = Instantiate(buttonPrefab, transform);
                    cameraButton.Setup(camera.Value.name, camera.Key, camera.Value.state == SecurityCameraManager.CameraVisibility.Locked);
                }
            }

            noCamerasText.enabled = cameraCount > 0;
        }
        else
        {
            noConnectionText.enabled = true;
            noCamerasText.enabled = false;
        }
    }
}
