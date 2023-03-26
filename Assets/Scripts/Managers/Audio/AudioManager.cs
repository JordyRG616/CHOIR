using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using TMPro;

public class AudioManager : MonoBehaviour
{
    #region Main
    private static AudioManager _instance;
    public static AudioManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<AudioManager>();
            return _instance;
        }
    }
    #endregion

    [SerializeField] private List<AudioChannel> channels;
    [SerializeField] private TextMeshProUGUI masterValue, weaponsValue, beatValue, sfxValue, uiValue;

    private void Start()
    {
        channels.ForEach(x => x.Initialize());
    }

    private void SetChannelVolume(string channelName, float volume)
    {
        var channel = channels.Find(x => x.channelName == channelName);
        channel.Volume = volume;
    }

    public void SetMasterVolume(float volume)
    {
        SetChannelVolume("Master", volume);
        masterValue.text = (volume * 100).ToString("00") + "%";
    }

    public void SetBeatVolume(float volume)
    {
        SetChannelVolume("BGM", volume);
        beatValue.text = (volume * 100).ToString("00") + "%";
    }

    public void SetSFXVolume(float volume)
    {
        SetChannelVolume("SFX", volume);
        sfxValue.text = (volume * 100).ToString("00") + "%";
    }

    public void SetWeaponsVolume(float volume)
    {
        SetChannelVolume("Weapons", volume);
        weaponsValue.text = (volume * 100).ToString("00") + "%";
    }

    public void SetUIVolume(float volume)
    {
        SetChannelVolume("General", volume);
        uiValue.text = (volume * 100).ToString("00") + "%";
    }

    public void PauseAudio()
    {
        channels.ForEach(x => x.Paused = true);
    }

    public void UnpauseAudio()
    {
        channels.ForEach(x => x.Paused = false);
    }
}

[System.Serializable]
public class AudioChannel
{
    public string channelName;
    public string busPath;
    public Bus bus;
    public float initialVolume;
    private float _volume;
    public float Volume
    {
        get => _volume;
        set
        {
            if (value < 0) _volume = 0;
            else if (value > 1) _volume = 1;
            else _volume = value;

            bus.setVolume(_volume);
        }
    }
    public bool Paused
    {
        get
        {
            bus.getPaused(out var paused);
            return paused;
        }
        set
        {
            bus.setPaused(value);
        }
    }

    public void Initialize()
    {
        bus = RuntimeManager.GetBus(busPath);
        Volume = initialVolume;
    }
}