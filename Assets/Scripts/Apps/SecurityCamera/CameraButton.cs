using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Button button;
    private string cameraName;

    public string CameraName { get => cameraName; set => cameraName = value; }

    public void Setup(string text, string cameraName, bool locked)
    {
        this.text.text = text;

        if (locked)
        {
            button.enabled = false;
            this.cameraName = cameraName + " (Bloqueado)";
        }
        else
        {
            this.cameraName = cameraName;
        }
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ShowCamera);
    }
    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    private async void ShowCamera()
    {
        GetComponentInParent<CamerasApp>().LoadState(AppsStates.CameraState.Camera);
        var texture = await SecurityCameraManager.Instance.LoadSecurityCamera(cameraName);
        GetComponentInParent<CamerasApp>().SetCamera(texture, cameraName);

    }
}
