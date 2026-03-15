using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthScripts : MonoBehaviour
{
    public GameManager gameManager;
    public float health;
    private float lerpTime;
    private float staminaLerpTime;
    public float maxHealth = 100f;

    private float chipSpeed = 2f;

    public Image backgroundHealthBar;
    public Image foregroundHealthBar;

    [Header("Stamina")]
    public float stamina;
    public float maxStamina = 100f;
    public Image backgroundStaminaBar;
    public Image foregroundStaminaBar;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;
    public float staminaRegenDelay = 1.5f;
    private float staminaRegenTimer;
    [HideInInspector] public bool isSprinting = false;

    public Image damageOverlay;
    public Image healOverlay;
    public float overlayDuration = 2f;
    private float durationTimer;
    public float fadeSpeed = 2f;
    private AudioSource[] playerSounds;
    private AudioSource healSound;
    private AudioSource hurtSound;

    private Color defaultStaminaColor;

void Start()
{
    health = maxHealth;
    stamina = maxStamina;
    playerSounds = GetComponents<AudioSource>();
    healSound = playerSounds[1];
    hurtSound = playerSounds[3];

    // cache the color set in Inspector
    if (foregroundStaminaBar != null)
        defaultStaminaColor = foregroundStaminaBar.color;
}

void HandleStamina()
{
    isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina > 0;

    if (isSprinting)
    {
        stamina -= staminaDrainRate * Time.deltaTime;
        staminaRegenTimer = 0f;

        if (foregroundStaminaBar != null)
            foregroundStaminaBar.color = stamina < 20f ? Color.red : defaultStaminaColor;
    }
    else
    {
        staminaRegenTimer += Time.deltaTime;
        if (staminaRegenTimer >= staminaRegenDelay)
            stamina += staminaRegenRate * Time.deltaTime;

        // restore original Inspector color
        if (foregroundStaminaBar != null)
            foregroundStaminaBar.color = defaultStaminaColor;
    }
}
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        HandleStamina();
        UpdateHealthUI();
        UpdateStaminaUI();

        if (damageOverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > overlayDuration)
            {
                float tempAlpha = damageOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempAlpha);
            }
        }

        if (healOverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > overlayDuration)
            {
                float tempAlpha = healOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, tempAlpha);
            }
        }

        if (health <= 0)
            gameManager.GameOver();
    }
    void UpdateStaminaUI()
    {
        if (backgroundStaminaBar == null || foregroundStaminaBar == null) return;

        float normalizedStamina = stamina / maxStamina;

        float fillB = backgroundStaminaBar.fillAmount;
        float fillF = foregroundStaminaBar.fillAmount;

        if (fillB > normalizedStamina)
        {
            foregroundStaminaBar.fillAmount = normalizedStamina;
            staminaLerpTime += Time.deltaTime;
            float percent = (staminaLerpTime / chipSpeed) * (staminaLerpTime / chipSpeed);
            backgroundStaminaBar.fillAmount = Mathf.Lerp(fillB, normalizedStamina, percent);
        }

        if (fillF < normalizedStamina)
        {
            backgroundStaminaBar.fillAmount = normalizedStamina;
            staminaLerpTime += Time.deltaTime;
            float percent = (staminaLerpTime / chipSpeed) * (staminaLerpTime / chipSpeed);
            foregroundStaminaBar.fillAmount = Mathf.Lerp(fillF, normalizedStamina, percent);
        }
    }

    public void UpdateHealthUI()
    {
        float fillAmountB = backgroundHealthBar.fillAmount;
        float fillAmountF = foregroundHealthBar.fillAmount;
        float normalizedHealth = health / maxHealth;

        if (fillAmountB > normalizedHealth)
        {
            foregroundHealthBar.fillAmount = normalizedHealth;
            backgroundHealthBar.color = Color.red;
            lerpTime += Time.deltaTime;
            float percentCompleted = lerpTime / chipSpeed;
            percentCompleted = percentCompleted * percentCompleted;
            backgroundHealthBar.fillAmount = Mathf.Lerp(fillAmountB, normalizedHealth, percentCompleted);
        }

        if (fillAmountF < normalizedHealth)
        {
            backgroundHealthBar.fillAmount = normalizedHealth;
            backgroundHealthBar.color = Color.green;
            lerpTime += Time.deltaTime;
            float percentCompleted = lerpTime / chipSpeed;
            percentCompleted = percentCompleted * percentCompleted;
            foregroundHealthBar.fillAmount = Mathf.Lerp(fillAmountF, normalizedHealth, percentCompleted);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        int random = Random.Range(1, 4);
        if (random == 2)
            hurtSound.Play();
        lerpTime = 0f;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0.2f);
        durationTimer = 0;
    }

    public void HealHealth(float heal)
    {
        health += heal;
        healSound.Play();
        lerpTime = 0f;
        durationTimer = 0;
        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 0.3f);
    }
}