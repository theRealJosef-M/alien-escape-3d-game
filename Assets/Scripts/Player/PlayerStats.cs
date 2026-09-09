using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private int coins = 0;
    private int level = 1;
    private int experience = 0;
    private int experienceToNextLevel = 100;

    private GameManager gameManager;
    private UIManager uiManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        uiManager.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        uiManager.UpdateHealth(currentHealth, maxHealth);
    }

    private void Die()
    {
        gameManager.PlayerCaught();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        uiManager.UpdateCoins(coins);
    }

    public void SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            uiManager.UpdateCoins(coins);
            return true;
        }
        return false;
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            level++;
            level = Mathf.Min(999, level);
            experienceToNextLevel = (int)(experienceToNextLevel * 1.1f);
            uiManager.UpdateLevel(level);
            uiManager.ShowLevelUpNotification(level);
        }
    }

    public void UpdateStamina(float staminaPercent)
    {
        uiManager.UpdateStamina(staminaPercent);
    }

    public int GetCoins() => coins;
    public int GetLevel() => level;
    public int GetExperience() => experience;
    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
