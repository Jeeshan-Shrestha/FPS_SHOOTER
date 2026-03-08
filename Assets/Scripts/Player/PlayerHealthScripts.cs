using UnityEngine;
using UnityEngine.UI;


public class PlayerHealthScripts : MonoBehaviour
{

    private float health;
    private float lerpTime;
    public float maxHealth = 100f;

    private float chipSpeed = 2f;

    public Image backgroundHealthBar;
    public Image foregroundHealthBar;

    public Image damageOverlay;
    public Image healOverlay;
    public float overlayDuration = 2f;
    private float durationTimer;
    public float fadeSpeed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health,0,maxHealth);
        UpdateHealthUI();
        if (damageOverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > overlayDuration)
            {
                float tempAplha = damageOverlay.color.a;
                tempAplha -= Time.deltaTime * fadeSpeed;
                damageOverlay.color = new Color(damageOverlay.color.r,damageOverlay.color.g,damageOverlay.color.b,tempAplha);

            }
        }
        if (healOverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > overlayDuration)
            {
                float tempAplha = healOverlay.color.a;
                tempAplha -= Time.deltaTime * fadeSpeed;
                healOverlay.color = new Color(damageOverlay.color.r,damageOverlay.color.g,damageOverlay.color.b,tempAplha);
            }
        }
        
    }

    public void UpdateHealthUI()
    {
        Debug.Log("Health: " + health);
        float fillAmountB = backgroundHealthBar.fillAmount;
        float fillAmountF = foregroundHealthBar.fillAmount;
        float normalizedHealth = health/maxHealth;
        if (fillAmountB > normalizedHealth)
        {
            foregroundHealthBar.fillAmount = normalizedHealth;
            backgroundHealthBar.color = Color.red;
            lerpTime += Time.deltaTime;
            float percentCompleted = lerpTime/chipSpeed;
            percentCompleted = percentCompleted * percentCompleted;
            backgroundHealthBar.fillAmount = Mathf.Lerp(fillAmountB,normalizedHealth,percentCompleted);
        }
        if (fillAmountF < normalizedHealth)
        {
            backgroundHealthBar.fillAmount = normalizedHealth;
            backgroundHealthBar.color = Color.green;
            lerpTime += Time.deltaTime;
            float percentCompleted = lerpTime/chipSpeed;
            percentCompleted = percentCompleted * percentCompleted;
            foregroundHealthBar.fillAmount = Mathf.Lerp(fillAmountF,normalizedHealth,percentCompleted);
        }
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTime = 0f;
        damageOverlay.color = new Color(damageOverlay.color.r,damageOverlay.color.g,damageOverlay.color.b,0.2f);
        durationTimer = 0;
    }

    public void HealHealth(float heal)
    {
        health += heal;
        lerpTime = 0f;
        durationTimer = 0;
        healOverlay.color = new Color(damageOverlay.color.r,damageOverlay.color.g,damageOverlay.color.b,0.3f);
    }
}
