using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Camera camera;
    private float xRotation = 0.0f;
    private float xSensitivity = 10.0f;
    private float ySensitivity = 20.0f;

    private float recoilX;
    private float recoilY;
    private float currentRecoilX;
    private float currentRecoilY;

    [Header("Recoil Settings")]
    public float recoilKickUp = 3f;
    public float recoilKickSide = 0.5f;
    public float recoilRecoverySpeed = 6f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // Smoothly interpolate recoil
        currentRecoilX = Mathf.Lerp(currentRecoilX, recoilX, recoilRecoverySpeed * Time.deltaTime);
        currentRecoilY = Mathf.Lerp(currentRecoilY, recoilY, recoilRecoverySpeed * Time.deltaTime);

        // Recover recoil back to zero
        recoilX = Mathf.Lerp(recoilX, 0f, recoilRecoverySpeed * Time.deltaTime);
        recoilY = Mathf.Lerp(recoilY, 0f, recoilRecoverySpeed * Time.deltaTime);

        // Apply recoil to camera
        float totalX = xRotation - currentRecoilX;
        totalX = Mathf.Clamp(totalX, -80f, 80f);
        camera.transform.localRotation = Quaternion.Euler(totalX, currentRecoilY, 0);
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    public void AddRecoil()
    {
        recoilX += recoilKickUp;
        recoilY += Random.Range(-recoilKickSide, recoilKickSide);
    }
}