﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translations : MonoBehaviour
{
    public Piece[] pieces;
    public Dictionary<string, Piece> dictionary;
    public SystemLanguage currentLanguage;
    public SystemLanguage[] availableLanguages;

    public string GetText(string id)
    {
        if (!dictionary.ContainsKey(id))
            return id;

        switch (currentLanguage)
        {
            case SystemLanguage.Spanish:
                return dictionary[id].es;
            default:
                return dictionary[id].en;
        }
    }

    void ChangeLanguage(int index)
    {
        currentLanguage = availableLanguages[index];
        SaveManager.Instance.Save("lang", index);
        foreach (var item in FindObjectsOfType<T_Translate>())
        {
            item.UpdateText();
        }
    }

    public void SetNextLanguage()
    {
        int currentIndex = 0;
        for (int i = 0; i < availableLanguages.Length; i++)
        {
            if (availableLanguages[i] == currentLanguage)
                currentIndex = i;
        }

        if (currentIndex == availableLanguages.Length - 1)
            ChangeLanguage(0);
        else
            ChangeLanguage(currentIndex + 1);
    }

    public void SetPrevLanguage()
    {
        int currentIndex = 0;
        for (int i = 0; i < availableLanguages.Length; i++)
        {
            if (availableLanguages[i] == currentLanguage)
                currentIndex = i;
        }

        if (currentIndex == 0)
            ChangeLanguage(availableLanguages.Length - 1);
        else
            ChangeLanguage(currentIndex - 1);
    }

    [Serializable]
    public struct Piece
    {
        public string id;

        public string en;
        public string es;
    }

    #region singleton
    //Singleton
    public static Translations instance;
    public void LoadTranslations()
    {
        dictionary = new Dictionary<string, Piece>();

        currentLanguage = availableLanguages[0]; //Set by default in case we cannot find the system language

        if (SaveManager.Instance.RetrieveInt("lang", -1) == -1)
        {
            for (int i = 0; i < availableLanguages.Length; i++)
            {
                if (Application.systemLanguage == availableLanguages[i])
                {
                    currentLanguage = Application.systemLanguage;
                    SaveManager.Instance.Save("lang", i);
                    break;
                }
            }
        }
        else
        {
            currentLanguage = availableLanguages[SaveManager.Instance.RetrieveInt("lang", 0)];
        }

        foreach (Piece piece in pieces)
        {
            dictionary.Add(piece.id, piece);
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
