using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public float damage = 50f;
    public float customGravity = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity += Vector3.down * customGravity * Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.gameObject.transform;

        if (collision.gameObject.CompareTag("Player"))
            hitTransform.GetComponent<PlayerHealthScripts>().TakeDamage(damage);
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Headshot"))
            hitTransform.GetComponent<HeadshotCollider>().TakeHeadshotDamage(damage);
        else if (collision.gameObject.CompareTag("Enemy"))
            hitTransform.GetComponent<EnemyHealthScript>().TakeDamage(damage);

        Destroy(gameObject);
    }
}