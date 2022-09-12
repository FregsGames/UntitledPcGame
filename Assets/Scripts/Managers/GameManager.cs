using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SerializedSingleton<GameManager>
{
    [SerializeField]
    private SystemEventManager gameEventManager;

    private bool firstMailSent = false;

    public void Initialize()
    {
        firstMailSent = SaveManager.Instance.RetrieveInt("firstEmailSent", 0) == 1;

        if (!firstMailSent)
        {
            gameEventManager.OnWifiEnabled += CheckFirstMail;
        }
    }

    private void CheckFirstMail(bool sent)
    {
        if(!firstMailSent && sent)
        {
            sent = true;
            gameEventManager.OnWifiEnabled -= CheckFirstMail;
            SaveManager.Instance.Save("firstEmailSent", 1);

            MailManager.Instance.Send("first");
        }
    }
}
