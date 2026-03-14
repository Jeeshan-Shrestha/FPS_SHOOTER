using TMPro;
using UnityEngine;

public class EnemyHealthScript: MonoBehaviour
{
    public float health = 100;
    public ParticleSystem bloodEffect;

    public GameManager gameManager;



    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        bloodEffect.Play();
        if (health <= 0)
        {
            gameManager.score += 1000;
            gameManager.killCount += 1;
            Destroy(gameObject);
        }
    } 
}