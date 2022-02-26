using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializator : MonoBehaviour
{
    [SerializeField]
    private SaveManager saveManager;
    [SerializeField]
    private IconsContainer desktopContainer;

    private void OnEnable()
    {
        saveManager.OnLoadFinished += InitializeDesktop;
    }


    private void OnDisable()
    {
        saveManager.OnLoadFinished -= InitializeDesktop;
    }


    private void InitializeDesktop()
    {
        desktopContainer.LoadState();
    }
}
