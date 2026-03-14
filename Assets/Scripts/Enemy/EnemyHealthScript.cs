using TMPro;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float health = 100;
    public ParticleSystem bloodEffect;
    public GameManager gameManager;

    private Enemy enemy;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
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
            Destroy(gameObject);
        }
    }
}