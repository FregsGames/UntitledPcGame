using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumericPopup : App
{
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Transform screenPanel;
    [SerializeField]
    private GameObject screenElementPrefab;

    public string Key { get; set; }
    public string CurrentInputKey { get; set; } = "";

    public Action<int> OnKeyPressed;

    private Dictionary<int, string> screenElements = new Dictionary<int, string>();

    public void Setup(string key)
    {
        Key = key;
        CleanScreenElements();
        InstantiateScreenElements(key);
    }

    private void InstantiateScreenElements(string key)
    {
        screenElements.Clear();

        for (int i = 0; i < key.Length; i++)
        {
            screenElements.Add(i, "");
            Instantiate(screenElementPrefab, screenPanel);
        }
    }

    private void CleanScreenElements()
    {
        for (int i = screenPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(screenPanel.GetChild(i).gameObject);
        }
    }
    private void ProcessInput(int key)
    {
        if(key == -1)
        {
            if(CurrentInputKey.Length > 0)
            {
                CurrentInputKey = CurrentInputKey.Substring(0, CurrentInputKey.Length - 1);
                screenPanel.GetChild(CurrentInputKey.Length).GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
        else if(CurrentInputKey.Length < Key.Length)
        {
            CurrentInputKey += key.ToString();
            screenPanel.GetChild(CurrentInputKey.Length -1).GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
        }
    }

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(CheckKey);
        cancelButton.onClick.AddListener(() => { systemEventManager.OnNumericPopUpSubmit?.Invoke(false); Close(); });

        OnKeyPressed += ProcessInput;
    }

    private void CheckKey()
    {
        systemEventManager.OnNumericPopUpSubmit?.Invoke(CurrentInputKey == Key);
        if (CurrentInputKey == Key) Close();
    }

    private void OnDisable()
    {
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        OnKeyPressed -= ProcessInput;
    }
}
