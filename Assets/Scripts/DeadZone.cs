using UnityEngine;

public class DeadZone : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.GameOver();
        }

        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null && enemyScript.enemyPath != null)
                Destroy(enemyScript.enemyPath.gameObject); // destroy path too

            Destroy(other.gameObject);
        }
    }
}