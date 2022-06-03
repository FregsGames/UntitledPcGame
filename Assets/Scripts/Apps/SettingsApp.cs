using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsApp : App, IStateableApp
{
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private TextMeshProUGUI volumeText;

    [SerializeField]
    private AppStateDictionary states;
    public string StateFamilyName { get => "SettingState"; }
    public Enum CurrentState { get => AppsStates.SettingState.Main; set => CurrentState = value; }
    public AppStateDictionary States => states;

    public void LoadState(Enum state)
    {
        if (state is not AppsStates.SettingState)
            return;

        foreach (var s in States.States)
        {
            s.Value.SetActive(s.Key == state);
        }
    }

    private void OnEnable()
    {
        soundSlider.SetValueWithoutNotify(SoundManager.Instance.Volume);
        volumeText.text = $"{Mathf.Round(SoundManager.Instance.Volume * 100)}%";
        soundSlider.onValueChanged.AddListener(AdjustVolume);
    }

    private void OnDisable()
    {
        soundSlider.onValueChanged.RemoveAllListeners();
    }

    public void AdjustVolume(float vol)
    {
        SoundManager.Instance.SetVolume(vol);
        volumeText.text = $"{Mathf.Round(SoundManager.Instance.Volume * 100)}%";
    }
}
