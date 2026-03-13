using UnityEngine;

public class EnemyHealthScript: MonoBehaviour
{
    public float health = 100;
    public ParticleSystem bloodEffect;


    void Start()
    {
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        bloodEffect.Play();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    } 
}