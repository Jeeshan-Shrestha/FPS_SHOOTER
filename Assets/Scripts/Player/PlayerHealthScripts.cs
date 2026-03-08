using Unity.VisualScripting;
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
            if (Input.GetKeyDown(KeyCode.J))
            {
                TakeDamage(Random.Range(5,10));
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                HealHealth(Random.Range(5,10));
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
        }

        public void HealHealth(float heal)
        {
            health += heal;
            lerpTime = 0f;
        }
    }
