using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ConfigureAudio : MonoBehaviour
{

    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey(EditPrefences.musicPropertyName) || !PlayerPrefs.HasKey(EditPrefences.soundPropertyName))
        {
            PlayerPrefs.SetFloat(EditPrefences.musicPropertyName, 1.0f);
            PlayerPrefs.SetFloat(EditPrefences.soundPropertyName, 1.0f);
            PlayerPrefs.Save();
        }

        float musicVolume = Mathf.Log10(PlayerPrefs.GetFloat(EditPrefences.musicPropertyName)) * 20;
        float soundVolume = Mathf.Log10(PlayerPrefs.GetFloat(EditPrefences.soundPropertyName)) * 20;

        audioMixer.SetFloat(EditPrefences.musicPropertyName, musicVolume);
        audioMixer.SetFloat(EditPrefences.soundPropertyName, soundVolume);
    }


}
