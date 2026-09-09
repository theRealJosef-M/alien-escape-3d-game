using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip[] ambientSounds;
    [SerializeField] private AudioClip alarmSound;
    [SerializeField] private AudioClip footstepSounds;
    [SerializeField] private AudioClip knockoutSound;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource sfxSource;

    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, volume * sfxVolume * masterVolume);
        }
    }

    public void PlayAlarm()
    {
        PlaySFX(alarmSound, 1f);
    }

    public void PlayKnockout()
    {
        PlaySFX(knockoutSound, 1f);
    }

    public void PlayVictory()
    {
        PlaySFX(victorySound, 1f);
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (backgroundMusicSource != null && musicClip != null)
        {
            backgroundMusicSource.clip = musicClip;
            backgroundMusicSource.volume = musicVolume * masterVolume;
            backgroundMusicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = musicVolume * masterVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}
