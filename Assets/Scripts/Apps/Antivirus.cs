using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Antivirus : App
{
    [SerializeField]
    private TextMeshProUGUI textToShow;

    [SerializeField]
    private Image image;

    [SerializeField]
    private float delayBetweenCharacters = 0.05f;

    private void OnEnable()
    {
        Computer.Instance.ComputerSettings.AntivirusEnabled = true;
        systemEventManager.OnVirusDefeated += ShowWinDialogue;
    }

    private void ShowWinDialogue(App obj)
    {
        SetText("¡Lo conseguimos!");
    }

    private void OnDisable()
    {
        Computer.Instance.ComputerSettings.AntivirusEnabled = false;
        systemEventManager.OnVirusDefeated -= ShowWinDialogue;

    }

    public void SetText(string text)
    {
        StartCoroutine(ShowText(text));
    }

    private IEnumerator ShowText(string text)
    {
        textToShow.text = "";

        foreach (var c in text)
        {
            textToShow.text += c;
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
    }

    public override void Close()
    {
        if (Computer.Instance.ComputerSettings.VirusActive)
        {
            SetText("¡No puedo cerrarme si hay un virus activo!");
            return;
        }

        base.Close();
    }
}
