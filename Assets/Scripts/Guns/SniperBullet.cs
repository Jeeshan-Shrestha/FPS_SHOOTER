using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public float damage = 50f;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.gameObject.transform;

        if (collision.gameObject.CompareTag("Player"))
        {
            hitTransform.GetComponent<PlayerHealthScripts>().TakeDamage(damage);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Headshot"))
        {
            hitTransform.GetComponent<HeadshotCollider>().TakeHeadshotDamage(damage);
            Debug.Log("SNIPER HEADSHOT!");
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            hitTransform.GetComponent<EnemyHealthScript>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}