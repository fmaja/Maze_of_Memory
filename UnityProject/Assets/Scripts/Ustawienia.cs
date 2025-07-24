using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        ApplyVolumes();

        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChanged(); });
    }

    void OnMusicVolumeChanged()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.musicSource.volume = musicSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        }
    }

    void OnSFXVolumeChanged()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.sfxSource.volume = sfxSlider.value;
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        }
    }

    void ApplyVolumes()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.musicSource.volume = musicSlider.value;
            AudioManager.Instance.sfxSource.volume = sfxSlider.value;
        }
    }
}
