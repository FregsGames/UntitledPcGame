using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private TextMeshProUGUI desc;

    public bool AlarmEnabled { get; set; }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(SetAlamrEnabled);
    }

    private void SetAlamrEnabled(bool state)
    {
        AlarmEnabled = state;
    }

    public void RemoveAlarm()
    {

    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    public void Setup(int hours, int minutes, bool enabled, string desc)
    {
        string hourFormatted = (hours < 10) ? $"0{hours}" : $"{hours}";
        string minutesFormatted = (minutes < 10) ? $"0{minutes}" : $"{minutes}";

        this.desc.text = desc;
        text.text = $"{hourFormatted}:{minutesFormatted}";
        toggle.SetIsOnWithoutNotify(enabled);
    }
}
