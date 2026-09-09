using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProgressData
{
    public int totalCoins = 0;
    public int currentLevel = 1;
    public int totalExperience = 0;
    public int winStreak = 0;
    public int dayStreak = 0;
    public int lastPlayDate = 0;
    public List<string> ownedUpgrades = new List<string>();
}

[System.Serializable]
public class LeaderboardEntry
{
    public int winStreak = 0;
    public int dayStreak = 0;
    public int level = 1;
}

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] private PlayerProgressData playerData = new PlayerProgressData();
    [SerializeField] private LeaderboardEntry leaderboard = new LeaderboardEntry();

    [Header("Upgrade Costs")]
    [SerializeField] private Dictionary<string, int> upgradeCosts = new Dictionary<string, int>()
    {
        { "Revive", 300 },
        { "CheckpointReturn", 500 },
        { "EnhancedVision", 200 },
        { "SilentSteps", 250 },
        { "ExtraSpeed", 200 }
    };

    private bool reviveUsedThisRun = false;

    private void Start()
    {
        LoadProgressData();
        CheckDayStreak();
    }

    private void LoadProgressData()
    {
        string json = PlayerPrefs.GetString("PlayerProgressData", JsonUtility.ToJson(playerData));
        playerData = JsonUtility.FromJson<PlayerProgressData>(json);
    }

    public void SaveProgressData()
    {
        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("PlayerProgressData", json);
        PlayerPrefs.Save();
    }

    public bool PurchaseUpgrade(string upgradeName)
    {
        if (!upgradeCosts.ContainsKey(upgradeName))
            return false;

        int cost = upgradeCosts[upgradeName];
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats.GetCoins() >= cost && !playerData.ownedUpgrades.Contains(upgradeName))
        {
            playerStats.SpendCoins(cost);
            playerData.ownedUpgrades.Add(upgradeName);
            SaveProgressData();
            return true;
        }

        return false;
    }

    public bool HasUpgrade(string upgradeName)
    {
        return playerData.ownedUpgrades.Contains(upgradeName);
    }

    public void UseRevive()
    {
        reviveUsedThisRun = true;
    }

    public bool ReviveUsedThisRun()
    {
        return reviveUsedThisRun;
    }

    public void UpdateLeaderboard(int winStreak, int dayStreak, int level)
    {
        leaderboard.winStreak = Mathf.Max(leaderboard.winStreak, winStreak);
        leaderboard.dayStreak = Mathf.Max(leaderboard.dayStreak, dayStreak);
        leaderboard.level = Mathf.Max(leaderboard.level, level);
        SaveProgressData();
    }

    private void CheckDayStreak()
    {
        int today = System.DateTime.Now.DayOfYear;
        int lastPlayDayOfYear = playerData.lastPlayDate;

        if (lastPlayDayOfYear == 0)
        {
            playerData.dayStreak = 1;
        }
        else if (lastPlayDayOfYear == today - 1)
        {
            playerData.dayStreak++;
        }
        else if (lastPlayDayOfYear != today)
        {
            playerData.dayStreak = 1;
        }

        playerData.lastPlayDate = today;
        SaveProgressData();
    }

    public PlayerProgressData GetProgressData() => playerData;
    public LeaderboardEntry GetLeaderboard() => leaderboard;
}
