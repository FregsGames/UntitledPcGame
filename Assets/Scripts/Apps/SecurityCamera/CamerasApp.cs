using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamerasApp : App, IStateableApp
{
    public Enum CurrentState { get => AppsStates.CameraState.General; set => CurrentState = value; }

    public string StateFamilyName { get => "SecurityCameraState"; }

    public AppStateDictionary States => states;

    [SerializeField]
    private AppStateDictionary states;

    [SerializeField]
    private RawImage rawImage;

    private string currentCameraName = "";

    public override void Close()
    {
        if (!string.IsNullOrEmpty(currentCameraName))
        {
            SecurityCameraManager.Instance.CloseCamera(currentCameraName);
        }
        base.Close();
    }

    public void LoadState(Enum state)
    {
        if (state is not AppsStates.CameraState)
            return;

        foreach (var s in States.States)
        {
            s.Value.gameObject.SetActive(s.Key.ToString() == state.ToString());
        }
    }

    public void SetCamera(Texture texure, string name)
    {
        currentCameraName = name;
        rawImage.texture = texure;
        systemEventManager.OnCameraOn?.Invoke();
    }

    public void BackButton()
    {
        systemEventManager.OnCameraOff?.Invoke();
        SecurityCameraManager.Instance.CloseCamera(currentCameraName);
        LoadState(AppsStates.CameraState.General);
        currentCameraName = "";
    }


}
