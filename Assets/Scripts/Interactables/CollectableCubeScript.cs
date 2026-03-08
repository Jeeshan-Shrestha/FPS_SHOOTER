using UnityEngine;

public class CollectableCubeScript : Interactable
{

    public PlayerHealthScripts playerHealthScripts;

    public GameObject particle;

    protected override void Interact()
    {
        Destroy(gameObject);
        playerHealthScripts.TakeDamage(10f);
        
    }
}
