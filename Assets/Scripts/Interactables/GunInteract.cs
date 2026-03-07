using UnityEngine;

public class GunInteract : Interactable
{

    public Transform cameraTransform; 
    protected override void Interact()
    {
        gameObject.transform.SetParent(cameraTransform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        Debug.Log("you have equipped the gun");
    }


}
