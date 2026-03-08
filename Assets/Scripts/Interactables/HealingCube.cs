using UnityEngine;

class HealingCube : Interactable
{
    
    public PlayerHealthScripts playerHealthScripts;
    public float healing;

    protected override void Interact()
    {
        playerHealthScripts.HealHealth(healing);
    }

}