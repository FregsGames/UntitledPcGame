using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FirstVirus_Intro : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI introText;

    [SerializeField]
    private string[] virusIntroText;

    [SerializeField]
    private string[] playerDefeatText;

    [SerializeField]
    private string[] playerWinText;

    [SerializeField]
    private float textDelay = 0.2f;

    private bool skip = false;
    private bool ended = false;

    public Action OnIntroEnded;
    public Action OnDefeatDialogueEnded;
    public Action OnWinDialogueEnded;

    [SerializeField]
    private GameObject cursor;


    public enum Dialogue {Intro, Defeat, Win }

    public void StartDialogue()
    {
        skip = false;
        ended = false;
        cursor.SetActive(false);

        StartCoroutine(ShowText(Dialogue.Intro));
        StartCoroutine(ListenPlayer());
    }

    public void DefeatDialogue()
    {
        skip = false;
        ended = false;
        cursor.SetActive(false);

        StartCoroutine(ShowText(Dialogue.Defeat));
        StartCoroutine(ListenPlayer());
    }

    public void WinDialogue()
    {
        skip = false;
        ended = false;
        cursor.SetActive(false);

        StartCoroutine(ShowText(Dialogue.Win));
        StartCoroutine(ListenPlayer());
    }

    private IEnumerator ShowText(Dialogue dialogue)
    {
        string[] textToUse = { };

        switch (dialogue)
        {
            case Dialogue.Intro:
                textToUse = virusIntroText;
                break;
            case Dialogue.Defeat:
                textToUse = playerDefeatText;
                break;
            case Dialogue.Win:
                textToUse = playerWinText;
                break;
            default:
                break;
        }

        foreach (var textFragment in textToUse)
        {
            introText.text = "";
            int index = 0;

            while(index < textFragment.Length)
            {
                if (skip)
                {
                    skip = false;
                    introText.text = textFragment;
                    break;
                }

                introText.text = introText.text + textFragment[index];
                index++;
                yield return new WaitForSeconds(textDelay);
            }

            cursor.SetActive(true);

            while (!skip)
            {
                yield return 0;
            }

            skip = false;
            cursor.SetActive(false);
        }
        ended = true;

        yield return new WaitForSeconds(2);


        switch (dialogue)
        {
            case Dialogue.Intro:
                OnIntroEnded?.Invoke();
                break;
            case Dialogue.Defeat:
                OnDefeatDialogueEnded?.Invoke();
                break;
            case Dialogue.Win:
                OnWinDialogueEnded?.Invoke();
                break;
            default:
                break;
        }
    }

    private IEnumerator ListenPlayer()
    {
        while (!ended)
        {
            if (Input.anyKeyDown)
            {
                skip = true;
            }

            yield return 0;
        }
    }

}
