﻿using System.Collections;
using TMPro;
using UnityEngine;

public class T_Translate : MonoBehaviour
{
    public string id;
    public TextMeshProUGUI customText; //if not asigned it will take the existing text in the gameobject if exists
    IEnumerator Start()
    {
        yield return null;
        UpdateText();
    }

    public void UpdateText()
    {
        if (customText == null)
        {
            if (GetComponent<TextMeshProUGUI>() != null)
            {
                GetComponent<TextMeshProUGUI>().text = Translations.instance.GetText(id).Replace("\\n", "\n");
            }
        }
        else
        {
            customText.text = Translations.instance.GetText(id).Replace("\\n", "\n");
        }
    }
}
