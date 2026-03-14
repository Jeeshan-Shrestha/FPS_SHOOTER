using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public bool sniperUnlocked = false;
    private int sniperPrice = 10000;

    public Button buyButton;
    public TextMeshProUGUI buyButtonText;

    private GameManager gameManager; // reference to get current score

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (sniperUnlocked)
        {
            buyButtonText.text = "OWNED";
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = (int)gameManager.score >= sniperPrice;
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
}