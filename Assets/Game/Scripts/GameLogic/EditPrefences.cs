using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EditPrefences : MonoBehaviour
{

    public const string soundPropertyName = "SoundsVolume";
    public const string musicPropertyName = "MusicVolume";

    private AudioSource _audioSource;
    private Slider _musicVolumeSlider;
    private Slider _soundVolumeSlider;

    public AudioMixer audioMixer;

    public AudioClip soundPreview;


    // Start is called before the first frame update
    void Start()
    {

        _musicVolumeSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
        _soundVolumeSlider = GameObject.Find("SoundVolumeSlider").GetComponent<Slider>();

        _musicVolumeSlider.value = PlayerPrefs.GetFloat(musicPropertyName);
        _soundVolumeSlider.value = PlayerPrefs.GetFloat(soundPropertyName);
        
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = soundPreview;
    }


    public void UpdateMusicVolume()
    {
        audioMixer.SetFloat(musicPropertyName, Mathf.Log10(_musicVolumeSlider.value)*20);
        PlayerPrefs.SetFloat(musicPropertyName, _musicVolumeSlider.value);
        PlayerPrefs.Save();
    }

    public void UpdateSoundVolume()
    {
        audioMixer.SetFloat(soundPropertyName, Mathf.Log10(_soundVolumeSlider.value)*20);
        _audioSource?.Play();
        PlayerPrefs.SetFloat(soundPropertyName, _soundVolumeSlider.value);
        PlayerPrefs.Save();
    }
}
