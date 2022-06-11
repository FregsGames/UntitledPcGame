using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumericPopup : App, IPopUp
{
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Transform screenPanel;
    [SerializeField]
    private GameObject screenElementPrefab;
    [SerializeField]
    private TextMeshProUGUI text;

    public string Key { get; set; }
    public string CurrentInputKey { get; set; } = "";

    public Action<int> OnKeyPressed;

    private Dictionary<int, string> screenElements = new Dictionary<int, string>();

    private bool sendNumbers = false;
    private int size = 0;

    public void Setup(string key)
    {
        Key = key;
        CleanScreenElements();
        InstantiateScreenElements(key);
        sendNumbers = false;
    }

    public void Setup(int count)
    {
        CleanScreenElements();
        InstantiateScreenElements(count);
        sendNumbers = true;
    }

    private void InstantiateScreenElements(string key)
    {
        InstantiateScreenElements(key.Length);
    }

    private void InstantiateScreenElements(int count)
    {
        size = count;
        screenElements.Clear();

        for (int i = 0; i < count; i++)
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
        else if(CurrentInputKey.Length < size)
        {
            CurrentInputKey += key.ToString();
            screenPanel.GetChild(CurrentInputKey.Length -1).GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
        }
    }

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(Send);
        cancelButton.onClick.AddListener(() => { systemEventManager.OnPopUpCancel?.Invoke(); Close(); });

        OnKeyPressed += ProcessInput;
    }

    private void Send()
    {
        if (!sendNumbers)
        {
            systemEventManager.OnNumericPopUpSubmit?.Invoke(CurrentInputKey == Key);
            if (CurrentInputKey == Key) Close();
        }
        else
        {
            systemEventManager.OnNumericPopUpNumberSubmit?.Invoke(CurrentInputKey);
            Close();
        }
    }

    private void OnDisable()
    {
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        OnKeyPressed -= ProcessInput;
    }
    public void SetText(string text)
    {
        this.text.text = text;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                int key = -1;
                if(int.TryParse(Input.inputString, out key))
                {
                    if(key < 9)
                    {
                        ProcessInput(key);
                    }
                }else if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    ProcessInput(-1);
                }
            }
        }
    }
}
