using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasApp : App, IStateableApp
{
    public Enum CurrentState { get => AppsStates.CameraState.General; set => CurrentState = value; }

    public string StateFamilyName { get => "SecurityCameraState"; }

    public AppStateDictionary States => states;

    [SerializeField]
    private AppStateDictionary states;

    public void LoadState(Enum state)
    {
        if (state is not AppsStates.CameraState)
            return;

        foreach (var s in States.States)
        {
            s.Value.SetActive(s.Key == state);
        }
    }


}
