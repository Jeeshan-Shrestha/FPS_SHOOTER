using UnityEngine;

public class EnemyHealthScript: MonoBehaviour
{
    public float health = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    } 
}