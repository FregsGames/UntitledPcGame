using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class TextApp : App
{
    [SerializeField]
    private TMP_InputField inputField;

    public override void Serialize()
    {
        base.Serialize();
        Dictionary<string, string> serialized = new Dictionary<string, string>();

        serialized.Add($"{ID}", ID);
        serialized.Add($"{ID}_content", inputField.text);

        SaveManager.Instance.Save(serialized, ID);
    }

    public override void Deserialize()
    {
        base.Deserialize();
        inputField.SetTextWithoutNotify(SaveManager.Instance.RetrieveString($"{ID}_content"));
    }

    public void SetText(string text)
    {
        inputField.text = text;
    }
}
