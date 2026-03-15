using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBarFill;
    private EnemyHealthScript healthScript;
    private Transform cam;
    private float maxHealth;

    void Start()
    {
        cam = Camera.main.transform;
        healthScript = GetComponentInParent<EnemyHealthScript>();
        maxHealth = healthScript.health;

        // assign event camera to canvas
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        // always face camera
        transform.LookAt(transform.position + cam.forward);

        // update fill
        healthBarFill.fillAmount = healthScript.health / maxHealth;
    }
}