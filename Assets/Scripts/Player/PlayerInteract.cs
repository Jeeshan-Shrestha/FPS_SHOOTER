using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField]
    private float rayDistance = 3.0f;
    private Camera camera;

    public LayerMask mask;

    private PlayerUI playerUI;

    private InputManager inputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.updateMessage(String.Empty);
        Ray ray = new Ray(camera.transform.position,camera.transform.forward);
        Debug.DrawRay(ray.origin,ray.direction * rayDistance);

        RaycastHit hitInfo;
        if(Physics.Raycast(ray,out hitInfo, rayDistance, mask)){

            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if ( interactable != null)
            {
                playerUI.updateMessage(interactable.promptMessage);
                if (inputManager.inFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
