using UnityEngine;
using System.Collections;

public enum GameDifficulty { Easy, Medium, Hard }

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameDifficulty currentDifficulty = GameDifficulty.Easy;
    [SerializeField] private int roomsPerDifficulty = 3;

    private PlayerController playerController;
    private PlayerStats playerStats;
    private UIManager uiManager;
    private ProgressionManager progressionManager;

    private bool isGameActive = false;
    private int currentRoomIndex = 0;
    private int totalRooms = 3;
    private int winStreak = 0;
    private int dayStreak = 0;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerStats = FindObjectOfType<PlayerStats>();
        uiManager = FindObjectOfType<UIManager>();
        progressionManager = FindObjectOfType<ProgressionManager>();

        SetupDifficulty();
        isGameActive = true;
    }

    private void SetupDifficulty()
    {
        switch (currentDifficulty)
        {
            case GameDifficulty.Easy:
                totalRooms = 3;
                break;
            case GameDifficulty.Medium:
                totalRooms = 5;
                break;
            case GameDifficulty.Hard:
                totalRooms = 7;
                break;
        }

        uiManager.UpdateDifficultyDisplay(currentDifficulty.ToString());
    }

    public void PlayerCaught()
    {
        isGameActive = false;
        StartCoroutine(PlayCaughtSequence());
    }

    private IEnumerator PlayCaughtSequence()
    {
        // Blur screen effect
        UIManager.Instance.PlayBlurEffect(2f);
        yield return new WaitForSeconds(2f);

        // Knockout animation
        // TODO: Implement knockout animation

        // Check for revive upgrade
        if (progressionManager.HasUpgrade("Revive") && !progressionManager.ReviveUsedThisRun())
        {
            progressionManager.UseRevive();
            uiManager.ShowNotification("Revived!");
            isGameActive = true;
        }
        else if (progressionManager.HasUpgrade("CheckpointReturn") && currentRoomIndex > 0)
        {
            uiManager.ShowNotification("Returning to checkpoint...");
            yield return new WaitForSeconds(1f);
            LoadRoom(currentRoomIndex - 1);
            isGameActive = true;
        }
        else
        {
            winStreak = 0;
            uiManager.ShowGameOverScreen();
            yield return new WaitForSeconds(3f);
            ReloadLevel();
        }
    }

    public void PlayerReachedCheckpoint()
    {
        currentRoomIndex++;
        uiManager.UpdateRoomProgress(currentRoomIndex, totalRooms);

        if (currentRoomIndex >= totalRooms)
        {
            PlayerEscaped();
        }
    }

    public void PlayerEscaped()
    {
        isGameActive = false;
        StartCoroutine(PlayVictorySequence());
    }

    private IEnumerator PlayVictorySequence()
    {
        int coinsEarned = GetCoinsForDifficulty();
        int experienceEarned = GetExperienceForDifficulty();

        playerStats.AddCoins(coinsEarned);
        playerStats.AddExperience(experienceEarned);

        winStreak++;
        dayStreak++;
        progressionManager.UpdateLeaderboard(winStreak, dayStreak, playerStats.GetLevel());

        uiManager.ShowVictoryScreen(coinsEarned, experienceEarned);
        yield return new WaitForSeconds(5f);

        ReloadLevel();
    }

    private int GetCoinsForDifficulty()
    {
        return currentDifficulty switch
        {
            GameDifficulty.Easy => 50,
            GameDifficulty.Medium => 100,
            GameDifficulty.Hard => 200,
            _ => 0
        };
    }

    private int GetExperienceForDifficulty()
    {
        return currentDifficulty switch
        {
            GameDifficulty.Easy => 25,
            GameDifficulty.Medium => 50,
            GameDifficulty.Hard => 100,
            _ => 0
        };
    }

    private void LoadRoom(int roomIndex)
    {
        currentRoomIndex = roomIndex;
        // TODO: Implement room loading
    }

    private void ReloadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public GameDifficulty GetCurrentDifficulty() => currentDifficulty;
    public bool IsGameActive() => isGameActive;
    public int GetCurrentRoomIndex() => currentRoomIndex;
    public int GetTotalRooms() => totalRooms;
}
