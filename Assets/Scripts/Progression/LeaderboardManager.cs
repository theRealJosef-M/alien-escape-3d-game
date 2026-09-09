using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardPlayer> winStreakLeaders = new List<LeaderboardPlayer>();
    public List<LeaderboardPlayer> dayStreakLeaders = new List<LeaderboardPlayer>();
    public List<LeaderboardPlayer> levelLeaders = new List<LeaderboardPlayer>();
}

[System.Serializable]
public class LeaderboardPlayer
{
    public string playerName = "Player";
    public int score = 0;
    public System.DateTime dateAchieved;
}

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private Transform winStreakContainer;
    [SerializeField] private Transform dayStreakContainer;
    [SerializeField] private Transform levelLeaderboardContainer;
    [SerializeField] private GameObject leaderboardEntryPrefab;

    private LeaderboardData leaderboardData = new LeaderboardData();
    private const int MAX_ENTRIES = 10;

    private void Start()
    {
        LoadLeaderboardData();
        RefreshLeaderboardDisplay();
    }

    public void AddWinStreakEntry(string playerName, int winStreak)
    {
        var entry = new LeaderboardPlayer
        {
            playerName = playerName,
            score = winStreak,
            dateAchieved = System.DateTime.Now
        };

        leaderboardData.winStreakLeaders.Add(entry);
        leaderboardData.winStreakLeaders = leaderboardData.winStreakLeaders
            .OrderByDescending(x => x.score)
            .Take(MAX_ENTRIES)
            .ToList();

        SaveLeaderboardData();
        RefreshLeaderboardDisplay();
    }

    public void AddDayStreakEntry(string playerName, int dayStreak)
    {
        var entry = new LeaderboardPlayer
        {
            playerName = playerName,
            score = dayStreak,
            dateAchieved = System.DateTime.Now
        };

        leaderboardData.dayStreakLeaders.Add(entry);
        leaderboardData.dayStreakLeaders = leaderboardData.dayStreakLeaders
            .OrderByDescending(x => x.score)
            .Take(MAX_ENTRIES)
            .ToList();

        SaveLeaderboardData();
        RefreshLeaderboardDisplay();
    }

    public void AddLevelEntry(string playerName, int level)
    {
        var entry = new LeaderboardPlayer
        {
            playerName = playerName,
            score = level,
            dateAchieved = System.DateTime.Now
        };

        leaderboardData.levelLeaders.Add(entry);
        leaderboardData.levelLeaders = leaderboardData.levelLeaders
            .OrderByDescending(x => x.score)
            .Take(MAX_ENTRIES)
            .ToList();

        SaveLeaderboardData();
        RefreshLeaderboardDisplay();
    }

    private void RefreshLeaderboardDisplay()
    {
        // Clear existing entries
        foreach (Transform child in winStreakContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in dayStreakContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in levelLeaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        // Display win streak leaderboard
        int rank = 1;
        foreach (var player in leaderboardData.winStreakLeaders)
        {
            CreateLeaderboardEntry(winStreakContainer, rank, player.playerName, player.score.ToString());
            rank++;
        }

        // Display day streak leaderboard
        rank = 1;
        foreach (var player in leaderboardData.dayStreakLeaders)
        {
            CreateLeaderboardEntry(dayStreakContainer, rank, player.playerName, player.score.ToString());
            rank++;
        }

        // Display level leaderboard
        rank = 1;
        foreach (var player in leaderboardData.levelLeaders)
        {
            CreateLeaderboardEntry(levelLeaderboardContainer, rank, player.playerName, $"Level {player.score}");
            rank++;
        }
    }

    private void CreateLeaderboardEntry(Transform container, int rank, string playerName, string score)
    {
        GameObject entry = Instantiate(leaderboardEntryPrefab, container);
        TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();

        if (texts.Length >= 3)
        {
            texts[0].text = rank.ToString();
            texts[1].text = playerName;
            texts[2].text = score;
        }
    }

    private void SaveLeaderboardData()
    {
        string json = JsonUtility.ToJson(leaderboardData);
        PlayerPrefs.SetString("LeaderboardData", json);
        PlayerPrefs.Save();
    }

    private void LoadLeaderboardData()
    {
        string json = PlayerPrefs.GetString("LeaderboardData", JsonUtility.ToJson(leaderboardData));
        leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);
    }

    public LeaderboardData GetLeaderboardData() => leaderboardData;
}
