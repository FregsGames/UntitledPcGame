using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class Translations : MonoBehaviour
{
    public Dictionary<string, string> currentDictionary;
    public SystemLanguage currentLanguage;
    public SystemLanguage[] availableLanguages;

    [SerializeField]
    private SystemEventManager eventManager;

    public void LoadTranslationsOfCurrentLang()
    {
        currentLanguage = availableLanguages[0]; //Set by default in case we cannot find the system language

        if (SaveManager.Instance.RetrieveInt("lang", -1) == -1)
        {
            currentLanguage = Application.systemLanguage;
        }
        else
        {
            currentLanguage = availableLanguages[SaveManager.Instance.RetrieveInt("lang", 0)];
        }

        currentDictionary = new Dictionary<string, string>();
        switch (currentLanguage)
        {
            default:
                var jsonTextFile = Resources.Load<TextAsset>("english");
                currentDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonTextFile.text);
                break;
        }

        eventManager.OnLanguagueLoaded?.Invoke();
    }

    public string GetText(string id)
    {
        if (!currentDictionary.ContainsKey(id))
            return id;

        return currentDictionary[id];
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


    #region singleton
    //Singleton
    public static Translations Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
