using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Camera cam;
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

    [Header("Scope Settings")]
    public float scopedFOV = 30f;
    public float normalFOV = 60f;
    public float scopeFOVSpeed = 10f;
    [HideInInspector] public bool isScoped = false;

    private float targetFOV;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        targetFOV = normalFOV;
        cam.fieldOfView = normalFOV;
    }

    void Update()
    {
        // Scope toggle
        if (Input.GetMouseButtonDown(1))
        {
            isScoped = !isScoped;
            targetFOV = isScoped ? scopedFOV : normalFOV;
        }

        // Smooth FOV transition
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, scopeFOVSpeed * Time.deltaTime);

        // Recoil (reduced while scoped)
        float recoilMult = isScoped ? 0.4f : 1f;

        currentRecoilX = Mathf.Lerp(currentRecoilX, recoilX, recoilRecoverySpeed * Time.deltaTime);
        currentRecoilY = Mathf.Lerp(currentRecoilY, recoilY, recoilRecoverySpeed * Time.deltaTime);

        recoilX = Mathf.Lerp(recoilX, 0f, recoilRecoverySpeed * Time.deltaTime);
        recoilY = Mathf.Lerp(recoilY, 0f, recoilRecoverySpeed * Time.deltaTime);

        float totalX = xRotation - (currentRecoilX * recoilMult);
        totalX = Mathf.Clamp(totalX, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(totalX, currentRecoilY * recoilMult, 0);
    }

    public void ProcessLook(Vector2 input)
    {
        // Sensitivity reduced while scoped
        float sensMultiplier = isScoped ? 0.4f : 1f;

        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity * sensMultiplier;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity * sensMultiplier);
    }

    public void AddRecoil()
    {
        recoilX += recoilKickUp;
        recoilY += Random.Range(-recoilKickSide, recoilKickSide);
    }
}