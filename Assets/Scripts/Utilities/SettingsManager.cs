using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;

    private void Start()
    {
        LoadSettings();
        SetupSliders();
        SetupToggles();
    }

    private void SetupSliders()
    {
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void SetupToggles()
    {
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
    }

    private void OnMasterVolumeChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void OnFullscreenToggled(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    private void LoadSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        int fullscreen = PlayerPrefs.GetInt("Fullscreen", 1);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
        fullscreenToggle.isOn = fullscreen == 1;

        AudioManager.Instance.SetMasterVolume(masterVolume);
        AudioManager.Instance.SetMusicVolume(musicVolume);
        AudioManager.Instance.SetSFXVolume(sfxVolume);
    }
}
