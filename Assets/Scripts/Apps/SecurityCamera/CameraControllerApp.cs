using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

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

    public void Connect()
    {
        sceneCamera = FindObjectOfType<SecurityCamera>();
    }

    private void OnEnable()
    {
        right.OnPressed += OnPressedRight;
        left.OnPressed += OnPressedLeft;
        up.OnPressed += OnPressedUp;
        down.OnPressed += OnPressedDown;
        

        Connect();
    }

    private void OnPressedRight()
    {
        StartCoroutine(MoveRight(right, false));
    }

    private void OnPressedLeft()
    {
        StartCoroutine(MoveRight(left, true));
    }

    private void OnPressedUp()
    {
        StartCoroutine(MoveUp(up, true));
    }

    private void OnPressedDown()
    {
        StartCoroutine(MoveUp(down, false));
    }

    private void OnDisable()
    {
        right.OnPressed -= OnPressedRight;
        left.OnPressed -= OnPressedLeft;
        up.OnPressed -= OnPressedUp;
        down.OnPressed -= OnPressedDown;
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
