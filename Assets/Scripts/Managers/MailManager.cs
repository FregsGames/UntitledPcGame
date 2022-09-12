using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MailManager : SerializedSingleton<MailManager>
{
    [SerializeField]
    private Dictionary<string, Mail> mailDB = new Dictionary<string, Mail>();

    private List<string> currentPlayerMails = new List<string>();

    public List<string> CurrentPlayerMails { get => currentPlayerMails; }

    [SerializeField]
    private SystemEventManager eventManager;

    public Mail GetMail(string id)
    {
        if (mailDB.ContainsKey(id))
        {
            return mailDB[id];
        }

        return null;
    }

    public void Send(string id)
    {
        if (currentPlayerMails.Contains(id))
        {
            Debug.LogWarning("Error trying to send an already existing mail");
            return;
        }

        if (!mailDB.ContainsKey(id))
        {
            Debug.LogWarning("Error trying to send an email that is not contained in mailDB: " + id);
        }

        currentPlayerMails.Add(id);
        Computer.Instance.NotificationCenter.RequestNotification("mail", "Nuevo correo", "Tienes un nuevo correo", null);
        eventManager.OnMailReceived?.Invoke(id);
        SaveManager.Instance.Save($"{id}_mailId",id);
    }

    public void Initialize()
    {
        currentPlayerMails.Clear();
        Dictionary<string, string> receivedMailDic = SaveManager.Instance.RetrieveStringThatContains("_mailId");
        currentPlayerMails.AddRange(receivedMailDic.Values);
    }

}

[Serializable]
public class Mail
{
    public string about;
    public string body;
    public string from;

    public string[] attachedFilesIds;
}