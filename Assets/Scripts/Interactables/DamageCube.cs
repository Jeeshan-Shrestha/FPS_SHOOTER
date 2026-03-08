using UnityEngine;

public class DamageCube : Interactable
{

    public PlayerHealthScripts playerHealthScripts;

    public float cubeDamage = 10f;

    protected override void Interact()
    {
        playerHealthScripts.TakeDamage(cubeDamage);
        
    }
}
