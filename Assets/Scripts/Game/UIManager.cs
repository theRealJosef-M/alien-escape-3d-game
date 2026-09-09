using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private TextMeshProUGUI roomProgressText;

    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject upgradeShopPanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Effects")]
    [SerializeField] private Image blurImage;
    [SerializeField] private CanvasGroup blurCanvasGroup;

    [Header("Notifications")]
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private CanvasGroup notificationCanvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Initialize UI
        blurCanvasGroup.alpha = 0;
        notificationCanvasGroup.alpha = 0;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = $"LEVEL {level}";
    }

    public void UpdateCoins(int coins)
    {
        coinsText.text = $"${coins}";
    }

    public void UpdateHealth(int current, int max)
    {
        healthText.text = $"{current}/{max}";
        healthBar.fillAmount = (float)current / max;
    }

    public void UpdateStamina(float staminaPercent)
    {
        staminaBar.fillAmount = staminaPercent;
    }

    public void UpdateRoomProgress(int current, int total)
    {
        roomProgressText.text = $"ROOM {current}/{total}";
    }

    public void UpdateDifficultyDisplay(string difficulty)
    {
        // TODO: Display difficulty indicator
    }

    public void PlayBlurEffect(float duration)
    {
        StartCoroutine(BlurEffectCoroutine(duration));
    }

    private IEnumerator BlurEffectCoroutine(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blurCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            yield return null;
        }
        blurCanvasGroup.alpha = 1;
    }

    public void ShowNotification(string message)
    {
        StartCoroutine(NotificationCoroutine(message));
    }

    private IEnumerator NotificationCoroutine(string message)
    {
        notificationText.text = message;
        notificationCanvasGroup.alpha = 1;
        yield return new WaitForSeconds(3f);
        notificationCanvasGroup.alpha = 0;
    }

    public void ShowLevelUpNotification(int newLevel)
    {
        ShowNotification($"LEVEL UP! You are now Level {newLevel}!");
    }

    public void ShowVictoryScreen(int coinsEarned, int experienceEarned)
    {
        victoryPanel.SetActive(true);
        victoryPanel.GetComponentInChildren<TextMeshProUGUI>().text = 
            $"ESCAPED!\n\n+{coinsEarned} Coins\n+{experienceEarned} Experience";
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }

    public void TogglePauseMenu()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
        Time.timeScale = pauseMenuPanel.activeSelf ? 0 : 1;
    }

    public void ToggleUpgradeShop()
    {
        upgradeShopPanel.SetActive(!upgradeShopPanel.activeSelf);
    }

    public void ToggleLeaderboard()
    {
        leaderboardPanel.SetActive(!leaderboardPanel.activeSelf);
    }
}
