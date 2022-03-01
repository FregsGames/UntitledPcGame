using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextApp : App
{
    [SerializeField]
    private TMP_InputField inputField;

    public override Dictionary<string, string> Serialize()
    {
        Dictionary<string, string> serialized = new Dictionary<string, string>();

        serialized.Add($"{ID}", ID);
        serialized.Add($"{ID}_content", inputField.text);

        return serialized;
    }

    public override void Deserialize()
    {
        inputField.SetTextWithoutNotify(SaveManager.instance.RetrieveString($"{ID}_content"));
    }
}
