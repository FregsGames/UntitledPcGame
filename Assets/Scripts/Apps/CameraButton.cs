using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CameraButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Button button;

    [SerializeField]
    private string cameraName;

    public string CameraName { get => cameraName; set => cameraName = value; }

    private void OnEnable()
    {
        button.onClick.AddListener(ShowCamera);
    }
    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    private void ShowCamera()
    {
        GetComponentInParent<CamerasApp>().LoadState(AppsStates.CameraState.Camera);
        SecurityCameraManager.Instance.LoadSecurityCamera(cameraName);
    }
}
