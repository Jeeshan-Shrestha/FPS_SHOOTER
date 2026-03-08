using UnityEngine;

public class GunInteract : Interactable
{

    public Transform cameraTransform; 
    protected override void Interact()
    {
        gameObject.transform.SetParent(cameraTransform);
        gameObject.transform.localPosition = new Vector3(0,-0.786f,1.077f);
        gameObject.transform.localRotation = Quaternion.identity;
        Debug.Log("you have equipped the gun");
    }


}
