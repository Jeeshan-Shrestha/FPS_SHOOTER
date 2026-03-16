using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public bool sniperUnlocked = false;
    private int sniperPrice = 10000;

    public Button SniperBuyButton;
    public TextMeshProUGUI SniperBuyButtonText;

    private GameManager gameManager; // reference to get current score

    public HealingCube healingCube;
    public int healingCubeCooldownReductionPrice = 20000;
    private bool healingCubeCooldownReductionUnlocked = false;
    public Button healingCubeButton;
    public TextMeshProUGUI healingCubeBuyButtonText;


    public int lifeStealPrice = 8000;
    public bool lifeStealUnlocked = false;
    public Button lifeStealButton;
    public TextMeshProUGUI lifeStealBuyButtonText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        healingCube = FindAnyObjectByType<HealingCube>();
        UpdateUI();
    }

    void Update()
    {
        if (!sniperUnlocked)
            SniperBuyButton.interactable = (int)gameManager.score >= sniperPrice;
        if (!healingCubeCooldownReductionUnlocked)
            healingCubeButton.interactable = (int)gameManager.score >= healingCubeCooldownReductionPrice;
        if (!lifeStealUnlocked)
            lifeStealButton.interactable = (int)gameManager.score >= lifeStealPrice;
    }

    void UpdateUI()
    {
        if (sniperUnlocked)
        {
            SniperBuyButtonText.text = "OWNED";
            SniperBuyButton.interactable = false;
        }
        else
        {
            SniperBuyButton.interactable = (int)gameManager.score >= sniperPrice;
        }
        if (healingCubeCooldownReductionUnlocked)
        {
            healingCubeBuyButtonText.text = "OWNED";
            healingCubeButton.interactable = false;
        }
        else
        {
            healingCubeButton.interactable = (int)gameManager.score >= healingCubeCooldownReductionPrice;
        }
        if (lifeStealUnlocked)
        {
            lifeStealBuyButtonText.text = "OWNED";
            lifeStealButton.interactable = false;
        }
        else
        {
            lifeStealButton.interactable = (int)gameManager.score >= lifeStealPrice;
        }
    }

    public void BuySniper()
    {
        if (sniperUnlocked) return;
        if ((int)gameManager.score < sniperPrice) return;

        gameManager.score -= sniperPrice;
        sniperUnlocked = true;
        UpdateUI();
        Debug.Log("Sniper purchased!");
    }

    public void BuyHealingCubeCooldownReduction()
    {
        if (healingCubeCooldownReductionUnlocked) return;
        if ((int)gameManager.score < healingCubeCooldownReductionPrice) return;

        gameManager.score -= healingCubeCooldownReductionPrice;
        healingCubeCooldownReductionUnlocked = true;
        healingCube.cooldownTime = 10f;
        UpdateUI();
    }

    public void BuyLifeSteal()
    {
        if (lifeStealUnlocked) return;
        if ((int)gameManager.score < lifeStealPrice) return;

        gameManager.score -= lifeStealPrice;
        lifeStealUnlocked = true;
        UpdateUI();
    }
}