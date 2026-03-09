using UnityEngine;

class CarInputManager: MonoBehaviour
{
    private MyPlayerInput myPlayerInput;
    public MyPlayerInput.InCarActions inCar;
    private CarMovement carMovement;

    void Awake()
    {
        myPlayerInput = new MyPlayerInput();
        inCar = myPlayerInput.InCar;
        carMovement = GetComponent<CarMovement>();
    }

    void Update()
    {
        carMovement.ProcessMovement(inCar.CarMovement.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        inCar.Enable();
    }

    void OnDisable()
    {
        inCar.Disable();
    }
}