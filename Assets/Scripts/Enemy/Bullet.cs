using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject,5f);
    }
    public void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.gameObject.transform;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            hitTransform.GetComponent<PlayerHealthScripts>().TakeDamage(10);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            hitTransform.GetComponent<EnemyHealthScript>().TakeDamage(10);
        }

        Destroy(gameObject);
    }

}
