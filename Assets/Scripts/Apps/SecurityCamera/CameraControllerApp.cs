using System.Collections;
using TMPro;
using UnityEngine;

public class CameraControllerApp : App
{
    private SecurityCamera sceneCamera;

    [SerializeField]
    private CameraControllerButton up;
    [SerializeField]
    private CameraControllerButton down;
    [SerializeField]
    private CameraControllerButton left;
    [SerializeField]
    private CameraControllerButton right;

    [SerializeField]
    private TextMeshProUGUI connectionText;

    public void Connect()
    {
        FindCamera();
    }

    private void OnEnable()
    {
        right.OnPressed += OnPressedRight;
        left.OnPressed += OnPressedLeft;
        up.OnPressed += OnPressedUp;
        down.OnPressed += OnPressedDown;
        systemEventManager.OnCameraOn += FindCamera;
        systemEventManager.OnCameraOff += DisableCamera;


        Connect();
    }

    private void DisableCamera()
    {
        sceneCamera = null;
        connectionText.text = "desconectado";
        connectionText.color = Color.red;
    }

    private void FindCamera()
    {
        sceneCamera = FindObjectOfType<SecurityCamera>();

        if(sceneCamera != null)
        {

            connectionText.text = "conectado";
            connectionText.color = Color.green;
        }
        else
        {
            connectionText.text = "desconectado";
            connectionText.color = Color.red;
        }
    }

    private void OnPressedRight()
    {
        if (sceneCamera == null)
            return;

        StartCoroutine(MoveRight(right, false));
    }

    private void OnPressedLeft()
    {
        if (sceneCamera == null)
            return;

        StartCoroutine(MoveRight(left, true));
    }

    private void OnPressedUp()
    {
        if (sceneCamera == null)
            return;

        StartCoroutine(MoveUp(up, true));
    }

    private void OnPressedDown()
    {
        if (sceneCamera == null)
            return;

        StartCoroutine(MoveUp(down, false));
    }

    private void OnDisable()
    {
        right.OnPressed -= OnPressedRight;
        left.OnPressed -= OnPressedLeft;
        up.OnPressed -= OnPressedUp;
        down.OnPressed -= OnPressedDown;

        systemEventManager.OnCameraOn -= FindCamera;
    }

    private IEnumerator MoveRight(CameraControllerButton button, bool right)
    {
        while (button.IsPressed)
        {
            sceneCamera.MoveHorizontal(right);
            yield return 0;
        }
    }

    private IEnumerator MoveUp(CameraControllerButton button, bool up)
    {
        while (button.IsPressed)
        {
            sceneCamera.MoveVertical(up);
            yield return 0;
        }

    }
}
