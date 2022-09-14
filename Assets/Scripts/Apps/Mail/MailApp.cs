using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailApp : App, IStateableApp
{
    public Enum CurrentState { get => AppsStates.MailState.General; set => CurrentState = value; }
    public string StateFamilyName { get => "MailState"; }

    public AppStateDictionary States => states;

    [SerializeField]
    private AppStateDictionary states;
    [SerializeField]
    private GameObject mailPrefab;
    [SerializeField]
    private SpritesDB spritesDB;
    [SerializeField]
    private Transform contanier;
    [SerializeField]
    private MailContent mailContent;
    [SerializeField]
    private UIEventManager uIEventManager;
    [SerializeField]
    private SystemEventManager eventManager;

    private Mail currentMail;

    public void LoadState(Enum state)
    {
        if (state is not AppsStates.MailState)
            return;

        foreach (var s in States.States)
        {
            s.Value.gameObject.SetActive(s.Key.ToString() == state.ToString());
        }

        if((AppsStates.MailState)state == AppsStates.MailState.Reading && currentMail != null)
        {
            mailContent.Setup(currentMail);
        }
    }

    public void BackToMain()
    {
        LoadState(AppsStates.MailState.General);
    }

    private void OnEnable()
    {
        if (!Computer.Instance.ComputerSettings.WifiEnabled)
        {
            LoadState(AppsStates.MailState.NoConnection);
        }
        else
        {
            LoadState(AppsStates.MailState.General);
        }

        MailPreviewButton[] mailPreviewButtons = GetComponentsInChildren<MailPreviewButton>();

        for (int i = mailPreviewButtons.Length - 1; i >= 0; i--)
        {
            Destroy(mailPreviewButtons[i].gameObject);
        }

        InstantiateMails();
        uIEventManager.OnMailButtonClicked += OpenMail;
        eventManager.OnMailReceived += InstantiateNewMail;
    }

    private void InstantiateNewMail(string mailId)
    {
        var mailGO = Instantiate(mailPrefab, contanier);
        Mail mail = MailManager.Instance.GetMail(mailId);

        mailGO.GetComponent<MailPreviewButton>().Setup(spritesDB.GetSprite("mail"), mail.about, mailId);
        mailGO.transform.SetAsFirstSibling();
    }

    private void InstantiateMails()
    {
        foreach (var mailId in MailManager.Instance.CurrentPlayerMails)
        {
            var mailGO = Instantiate(mailPrefab, contanier);
            Mail mail = MailManager.Instance.GetMail(mailId);

            mailGO.GetComponent<MailPreviewButton>().Setup(spritesDB.GetSprite("mail"), mail.about, mailId);
            mailGO.transform.SetAsFirstSibling();
        }
    }

    private void OpenMail(string mailId)
    {
        currentMail = MailManager.Instance.GetMail(mailId);
        if(currentMail != null)
        {
            LoadState(AppsStates.MailState.Reading);
        }
        else
        {
            Debug.LogWarning("Mail not found");
        }
    }

    private void OnDisable()
    {
        uIEventManager.OnMailButtonClicked -= OpenMail;
        eventManager.OnMailReceived -= InstantiateNewMail;
    }
}
