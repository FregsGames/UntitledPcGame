using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    Dictionary<string, int> intSaves;
    Dictionary<string, float> floatSaves;
    Dictionary<string, string> stringSaves;

    string intFile = "/intSaves.txt";
    string floatFile = "/floatSaves.txt";
    string stringFile = "/stringSaves.txt";

    public Action OnLoadFinished;

    public bool SaveDataFound { get; set; }

    // Odin Stuff
    [PropertySpace(10, 0)]
    [Button("Clear Save Data", ButtonSizes.Medium)]
    protected void ClearSaved()
    {
        File.Delete(Application.persistentDataPath + intFile);
        File.Delete(Application.persistentDataPath + floatFile);
        File.Delete(Application.persistentDataPath + stringFile);
    }

    public void RemoveEntriesThatContains(string fragment)
    {
        List<string> keys = stringSaves.Where(e => e.Key.Contains(fragment))?.Select(e => e.Key).ToList();

        foreach (string key in keys)
        {
            stringSaves.Remove(key);
        }

        keys = intSaves.Where(e => e.Key.Contains(fragment))?.Select(e => e.Key).ToList();

        foreach (string key in keys)
        {
            stringSaves.Remove(key);
        }

        keys = floatSaves.Where(e => e.Key.Contains(fragment))?.Select(e => e.Key).ToList();
        foreach (string key in keys)
        {
            stringSaves.Remove(key);
        }
    }

    public void Save(string id, int value)
    {
        if (intSaves.ContainsKey(id))
            intSaves[id] = value;
        else
            intSaves.Add(id, value);

    }
    public void Save(string id, float value)
    {
        if (floatSaves.ContainsKey(id))
            floatSaves[id] = value;
        else
            floatSaves.Add(id, value);

    }

    public void Save(string id, string value)
    {
        if (stringSaves.ContainsKey(id))
            stringSaves[id] = value;
        else
            stringSaves.Add(id, value);
    }

    public void Save(Dictionary<string, string> toSave, string id)
    {
        RemoveEntriesThatContains(id);

        foreach (var item in toSave)
        {
            Save(item.Key, item.Value);
        }
    }

    public async Task SaveChanges()
    {
        await Task.WhenAll(WriteStrings(), WriteFloats(), WriteInts());
    }

    public int RetrieveInt(string id)
    {
        if (intSaves.ContainsKey(id))
            return intSaves[id];
        else
            return 0;
    }
    public int RetrieveInt(string id, int def)
    {
        if (intSaves.ContainsKey(id))
            return intSaves[id];
        else
            return def;
    }
    public float RetrieveFloat(string id)
    {
        if (floatSaves.ContainsKey(id))
            return floatSaves[id];
        else
            return 0.0f;
    }
    public float RetrieveFloat(string id, float def)
    {
        if (floatSaves.ContainsKey(id))
            return floatSaves[id];
        else
            return def;
    }

    public string RetrieveString(string id)
    {
        if (stringSaves.ContainsKey(id))
            return stringSaves[id];
        else
            return "";
    }

    public Dictionary<string, string> RetrieveStringThatContains(string idFragment)
    {
        return stringSaves.Where(item => item.Key.Contains(idFragment)).ToDictionary(t => t.Key, t => t.Value);
    }
    public Dictionary<string, string> RetrieveStringThatEndsWith(string idFragment)
    {
        return stringSaves.Where(item => item.Key.EndsWith(idFragment)).ToDictionary(t => t.Key, t => t.Value);
    }

    private async Task WriteInts()
    {
        string intJson = JsonConvert.SerializeObject(intSaves);
        await File.WriteAllTextAsync(Application.persistentDataPath + intFile, intJson);
    }
    private async Task WriteFloats()
    {
        string floatJson = JsonConvert.SerializeObject(floatSaves);
        await File.WriteAllTextAsync(Application.persistentDataPath + floatFile, floatJson);
    }
    private async Task WriteStrings()
    {
        string stringJson = JsonConvert.SerializeObject(stringSaves, Formatting.Indented);
        await File.WriteAllTextAsync(Application.persistentDataPath + stringFile, stringJson);
    }

    private bool Read()
    {
        if (!File.Exists(Application.persistentDataPath + intFile))
        {
            File.WriteAllText(Application.persistentDataPath + intFile, "");
        }

        StreamReader reader = new StreamReader(Application.persistentDataPath + intFile, true);
        string jsonInts = reader.ReadToEnd();
        intSaves = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonInts);

        if (intSaves == null)
        {
            intSaves = new Dictionary<string, int>();
        }

        if (!File.Exists(Application.persistentDataPath + floatFile))
        {
            File.WriteAllText(Application.persistentDataPath + floatFile, "");
        }
        reader.Close();
        reader = new StreamReader(Application.persistentDataPath + floatFile, true);

        string jsonFloats = reader.ReadToEnd();
        floatSaves = JsonConvert.DeserializeObject<Dictionary<string, float>>(jsonFloats);

        if (floatSaves == null)
        {
            floatSaves = new Dictionary<string, float>();
        }

        if (!File.Exists(Application.persistentDataPath + stringFile))
        {
            File.WriteAllText(Application.persistentDataPath + stringFile, "");
        }
        reader.Close();
        reader = new StreamReader(Application.persistentDataPath + stringFile, true);
        string jsonStrings = reader.ReadToEnd();
        stringSaves = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStrings);

        if (stringSaves == null)
        {
            stringSaves = new Dictionary<string, string>();
        }


        reader.Close();

        OnLoadFinished?.Invoke();

        return stringSaves.Count != 0 || floatSaves.Count != 0 || intSaves.Count != 0;
    }

    #region singleton
    //Singleton
    public static SaveManager Instance;
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

    public bool LoadSave()
    {
        SaveDataFound = Read();
        Translations.Instance.LoadTranslationsOfCurrentLang();
        SoundManager.Instance.LoadSettings();
        return SaveDataFound;
    }

    #endregion
}