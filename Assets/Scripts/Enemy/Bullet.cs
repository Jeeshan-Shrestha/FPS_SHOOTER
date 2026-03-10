using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        Transform hitTransfrom = collision.gameObject.transform;
        if(collision.gameObject.CompareTag("Player")){
            Debug.Log("Hit Player");
            hitTransfrom.GetComponent<PlayerHealthScripts>().TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
