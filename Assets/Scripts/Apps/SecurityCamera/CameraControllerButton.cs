using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraControllerButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    public Action OnPressed;

    public bool IsPressed { get; set; } = false;

    public void OnPress()
    {
        IsPressed = true;
        OnPressed?.Invoke();
    }

    public void OnRelease()
    {
        IsPressed = false;
    }

}
