using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private MyPlayerInput myPlayerInput;
    public MyPlayerInput.InFootActions inFoot;

    private PlayerMovement playerMovement;

    private PlayerLook playerLook;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
        myPlayerInput = new MyPlayerInput();
        inFoot = myPlayerInput.InFoot;
        playerMovement = GetComponent<PlayerMovement>();
        playerLook = GetComponent<PlayerLook>();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        from the infoot object we created in the input actions
        we will read the value of action movement which was Vector2 when we 
        created it in the unity using input actions and then pass it as the 
        parameter of the processMovement method that moves our player
        */
        
        playerMovement.ProcessMovement(inFoot.Movement.ReadValue<Vector2>());
        inFoot.Jump.performed += ctx => playerMovement.Jump();
        playerLook.ProcessLook(inFoot.Look.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        inFoot.Enable();
    }

    void OnDisable()
    {
        inFoot.Disable();      
    }


}
