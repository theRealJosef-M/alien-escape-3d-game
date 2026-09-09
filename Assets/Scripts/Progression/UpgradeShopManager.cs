using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class Upgrade
{
    public string upgradeName;
    public int cost;
    public string description;
    public Sprite icon;
    public bool isPermanent = true;
}

public class UpgradeShopManager : MonoBehaviour
{
    [SerializeField] private List<Upgrade> availableUpgrades = new List<Upgrade>();
    [SerializeField] private Transform shopContainer;
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private TextMeshProUGUI playerCoinsText;

    private PlayerStats playerStats;
    private ProgressionManager progressionManager;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        progressionManager = FindObjectOfType<ProgressionManager>();

        InitializeUpgrades();
        DisplayUpgrades();
    }

    private void InitializeUpgrades()
    {
        availableUpgrades = new List<Upgrade>
        {
            new Upgrade
            {
                upgradeName = "Revive",
                cost = 300,
                description = "Get an extra life when caught",
                isPermanent = false
            },
            new Upgrade
            {
                upgradeName = "CheckpointReturn",
                cost = 500,
                description = "Return to last checkpoint instead of restarting",
                isPermanent = true
            },
            new Upgrade
            {
                upgradeName = "EnhancedVision",
                cost = 200,
                description = "See guard patrol routes through walls",
                isPermanent = true
            },
            new Upgrade
            {
                upgradeName = "SilentSteps",
                cost = 250,
                description = "Reduce your detection radius by 50%",
                isPermanent = true
            },
            new Upgrade
            {
                upgradeName = "ExtraSpeed",
                cost = 200,
                description = "Increase movement speed by 20%",
                isPermanent = true
            },
            new Upgrade
            {
                upgradeName = "HealthBoost",
                cost = 150,
                description = "Increase max health by 25",
                isPermanent = true
            }
        };
    }

    private void DisplayUpgrades()
    {
        // Clear existing items
        foreach (Transform child in shopContainer)
        {
            Destroy(child.gameObject);
        }

        // Display each upgrade
        foreach (var upgrade in availableUpgrades)
        {
            CreateUpgradeItem(upgrade);
        }
    }

    private void CreateUpgradeItem(Upgrade upgrade)
    {
        GameObject item = Instantiate(upgradeItemPrefab, shopContainer);
        
        TextMeshProUGUI nameText = item.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionText = item.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI costText = item.transform.Find("CostText").GetComponent<TextMeshProUGUI>();
        Button buyButton = item.transform.Find("BuyButton").GetComponent<Button>();
        Image iconImage = item.transform.Find("IconImage").GetComponent<Image>();

        nameText.text = upgrade.upgradeName;
        descriptionText.text = upgrade.description;
        costText.text = $"${upgrade.cost}";

        bool alreadyOwned = progressionManager.HasUpgrade(upgrade.upgradeName);
        buyButton.interactable = !alreadyOwned && playerStats.GetCoins() >= upgrade.cost;

        if (alreadyOwned)
        {
            costText.text = "OWNED";
            buyButton.interactable = false;
        }

        buyButton.onClick.AddListener(() => PurchaseUpgrade(upgrade));
    }

    private void PurchaseUpgrade(Upgrade upgrade)
    {
        if (progressionManager.PurchaseUpgrade(upgrade.upgradeName))
        {
            DisplayUpgrades();
            playerCoinsText.text = $"Coins: ${playerStats.GetCoins()}";
        }
    }
}
