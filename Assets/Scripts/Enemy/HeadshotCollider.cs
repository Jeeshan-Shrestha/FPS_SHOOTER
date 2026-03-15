using UnityEngine;

public class HeadshotCollider : MonoBehaviour
{
    public float damageMultiplier = 2.5f;
    private EnemyHealthScript enemyHealth;

    void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealthScript>();
    }

    public void TakeHeadshotDamage(float damage)
    {
        enemyHealth.TakeDamage(damage * damageMultiplier);
        FindAnyObjectByType<GameManager>().ShowHeadshotIndicator();
    }
}