using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpeningCutsceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoDisplay;
    [SerializeField] private float transitionDuration = 2f;

    private CharacterGender playerGender;
    private GameDifficulty difficulty;

    private void Start()
    {
        playerGender = (CharacterGender)PlayerPrefs.GetInt("SelectedGender", 0);
        difficulty = (GameDifficulty)PlayerPrefs.GetInt("SelectedDifficulty", 0);

        // Load appropriate video based on gender
        string videoPath = playerGender switch
        {
            CharacterGender.Male => "Videos/abduction_male",
            CharacterGender.Female => "Videos/abduction_female",
            CharacterGender.Other => "Videos/abduction_other",
            _ => "Videos/abduction_male"
        };

        // Load and play video
        VideoClip clip = Resources.Load<VideoClip>(videoPath);
        if (clip != null)
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();
            StartCoroutine(WaitForVideoEnd());
        }
        else
        {
            Debug.LogWarning($"Video not found: {videoPath}");
            SkipCutscene();
        }
    }

    private IEnumerator WaitForVideoEnd()
    {
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // Video finished playing
        yield return new WaitForSeconds(1f);
        SkipCutscene();
    }

    private void Update()
    {
        // Allow skipping cutscene with any key
        if (Input.anyKeyDown && Input.GetKeyDown(KeyCode.Space))
        {
            SkipCutscene();
        }
    }

    private void SkipCutscene()
    {
        videoPlayer.Stop();
        SceneManager.LoadScene("GameplayLevel");
    }
}
