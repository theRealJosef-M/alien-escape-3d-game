using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum CharacterGender { Male, Female, Other }

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button maleButton;
    [SerializeField] private Button femaleButton;
    [SerializeField] private Button otherButton;

    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    [SerializeField] private Button startButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject genderSelectionPanel;
    [SerializeField] private GameObject difficultySelectionPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject leaderboardPanel;

    [SerializeField] private RawImage cinematicVideo;
    [SerializeField] private VideoPlayer videoPlayer;

    private CharacterGender selectedGender = CharacterGender.Male;
    private GameDifficulty selectedDifficulty = GameDifficulty.Easy;

    private void Start()
    {
        maleButton.onClick.AddListener(() => SelectGender(CharacterGender.Male));
        femaleButton.onClick.AddListener(() => SelectGender(CharacterGender.Female));
        otherButton.onClick.AddListener(() => SelectGender(CharacterGender.Other));

        easyButton.onClick.AddListener(() => SelectDifficulty(GameDifficulty.Easy));
        mediumButton.onClick.AddListener(() => SelectDifficulty(GameDifficulty.Medium));
        hardButton.onClick.AddListener(() => SelectDifficulty(GameDifficulty.Hard));

        startButton.onClick.AddListener(StartGame);
        leaderboardButton.onClick.AddListener(ToggleLeaderboard);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);

        mainMenuPanel.SetActive(true);
        genderSelectionPanel.SetActive(false);
        difficultySelectionPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }

    private void SelectGender(CharacterGender gender)
    {
        selectedGender = gender;
        // Highlight selected button
        difficultySelectionPanel.SetActive(true);
    }

    private void SelectDifficulty(GameDifficulty difficulty)
    {
        selectedDifficulty = difficulty;
        // Highlight selected button
    }

    private void StartGame()
    {
        // Save selections
        PlayerPrefs.SetInt("SelectedGender", (int)selectedGender);
        PlayerPrefs.SetInt("SelectedDifficulty", (int)selectedDifficulty);
        PlayerPrefs.Save();

        // Load opening cutscene
        SceneManager.LoadScene("OpeningCutscene");
    }

    private void ToggleLeaderboard()
    {
        leaderboardPanel.SetActive(!leaderboardPanel.activeSelf);
    }

    private void OpenSettings()
    {
        // TODO: Implement settings menu
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
