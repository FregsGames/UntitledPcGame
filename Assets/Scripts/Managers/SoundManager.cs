using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SerializedMonoBehaviour
{
    public enum Sound
    {
        AppOpen,
        PopUpOpen,
        Cancel,
        Notification
    }

    [SerializeField]
    private AudioSource soundAudioSource;
    [SerializeField]
    private Dictionary<Sound, AudioClip> sounds = new Dictionary<Sound, AudioClip>();

    public float Volume { get; private set; } = 1;

    public void SetVolume(float vol)
    {
        Volume = vol;
        soundAudioSource.volume = vol;
        SaveManager.Instance.Save("volume", vol);
    }

    public void PlaySound(Sound sound, float pitch = 1)
    {
        soundAudioSource.pitch = pitch;

        AudioClip audioClip;
        if (sounds.TryGetValue(sound, out audioClip))
        {
            soundAudioSource.PlayOneShot(audioClip);
            //Debug.Log($"[AUDIO] playing {audioClip.name}");
        }
    }

    public void LoadSettings()
    {
        SetVolume(SaveManager.Instance.RetrieveFloat("volume", 1f));
    }

    #region Singleton
    protected static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    var singletonObj = new GameObject();
                    singletonObj.name = typeof(SoundManager).ToString();
                    instance = singletonObj.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = GetComponent<SoundManager>();

        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
