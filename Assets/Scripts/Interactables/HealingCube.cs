using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealingCube : Interactable
{
    public PlayerHealthScripts playerHealthScripts;
    public float healing;

    [Header("Cooldown")]
    public float cooldownTime = 10f;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    [Header("World Space UI")]
    public TextMeshProUGUI cooldownText;
    public Image cooldownFill;
    public GameObject readyIndicator;

    private Canvas worldCanvas;

    void Start()
    {
        worldCanvas = GetComponentInChildren<Canvas>();
        if (worldCanvas != null)
            worldCanvas.worldCamera = Camera.main;

        if (cooldownText != null) cooldownText.text = "READY";
        if (cooldownFill != null) cooldownFill.fillAmount = 1f;
        if (readyIndicator != null) readyIndicator.SetActive(true);
    }

    protected override void Interact()
    {
        if (isOnCooldown) return;

        playerHealthScripts.HealHealth(healing);
        isOnCooldown = true;
        cooldownTimer = cooldownTime;

        if (readyIndicator != null) readyIndicator.SetActive(false);
    }

    void Update()
    {
        if (!isOnCooldown) return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownText != null)
            cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString() + "s";

        if (cooldownFill != null)
            cooldownFill.fillAmount = cooldownTimer / cooldownTime;

        if (cooldownTimer <= 0f)
        {
            isOnCooldown = false;
            cooldownTimer = 0f;

            if (cooldownText != null) cooldownText.text = "READY";
            if (cooldownFill != null) cooldownFill.fillAmount = 1f;
            if (readyIndicator != null) readyIndicator.SetActive(true);
        }
    }
}