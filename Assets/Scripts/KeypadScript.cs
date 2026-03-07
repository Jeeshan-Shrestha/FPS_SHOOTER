using System;
using UnityEngine;

public class KeypadScript : Interactable
{

    [SerializeField]
    private GameObject door;
    private Boolean isDoorOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected override void Interact()
    {
        isDoorOpen = !isDoorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen",isDoorOpen); 
    }
}
