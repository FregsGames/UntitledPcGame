using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MailPreviewButton : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Button button;
    [SerializeField]
    private UIEventManager uIEventManager;

    public string MailId { get; set; }

    public void Setup(Sprite sprite, string text, string id)
    {
        image.sprite = sprite;
        this.text.text = text;
        MailId = id;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OpenMail);
    }

    public void OpenMail()
    {
        uIEventManager.OnMailButtonClicked?.Invoke(MailId);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
