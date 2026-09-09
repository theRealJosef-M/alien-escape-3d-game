using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [System.Serializable]
    public class GameSaveData
    {
        public int playerLevel = 1;
        public int totalCoins = 0;
        public int totalExperience = 0;
        public int winStreak = 0;
        public int dayStreak = 0;
        public string[] ownedUpgrades = new string[0];
    }

    private GameSaveData saveData = new GameSaveData();

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

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString("GameSaveData", json);
        PlayerPrefs.Save();
        Debug.Log("Game saved successfully!");
    }

    public void LoadGame()
    {
        string json = PlayerPrefs.GetString("GameSaveData", JsonUtility.ToJson(new GameSaveData(), true));
        saveData = JsonUtility.FromJson<GameSaveData>(json);
        Debug.Log("Game loaded successfully!");
    }

    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey("GameSaveData");
        saveData = new GameSaveData();
        Debug.Log("Save data deleted!");
    }

    public GameSaveData GetSaveData() => saveData;
    public void SetSaveData(GameSaveData data) => saveData = data;
}
