using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    private Camera camera;
    private float xRotation = 0.0f;
    private float xSensitivity = 10.0f;
    private float ySensitivity = 20.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = GetComponentInChildren<Camera>();
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation,-80,80);
        camera.transform.localRotation = Quaternion.Euler(xRotation,0,0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        
    }
}
