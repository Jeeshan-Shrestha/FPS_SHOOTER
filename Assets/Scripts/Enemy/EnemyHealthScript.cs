using TMPro;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float health = 100;
    public ParticleSystem bloodEffect;
    public GameManager gameManager;

    private Enemy enemy;
    public ShopManager shopManager;
    public PlayerHealthScripts playerHealthScripts;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        shopManager = FindAnyObjectByType<ShopManager>();
        playerHealthScripts = FindAnyObjectByType<PlayerHealthScripts>();
        enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        bloodEffect.Play();

        if (enemy != null)
            enemy.OnHit();

        if (health <= 0)
        {
            gameManager.score += 1000;
            gameManager.killCount += 1;
            if (shopManager.lifeStealUnlocked)
            {
                playerHealthScripts.HealHealth(20);
            }
            Destroy(gameObject);
        }
    }
}