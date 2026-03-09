using System;
using UnityEngine;

class CarManager : Interactable
{
    public GameObject player;
    public MyPlayerInput playerInput;

    public PlayerMovement playerMovement;
    protected override void Interact()
    { 
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled =false;
        Debug.Log("Player has entered the car");
        Vector3 pos = new Vector3(gameObject.transform.position.x,71f,gameObject.transform.position.z);
        player.transform.position = pos;
        player.transform.rotation = gameObject.transform.rotation;
        player.transform.SetParent(transform);
        cc.enabled = true;
    }

}