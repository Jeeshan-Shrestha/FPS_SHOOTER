using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLook : MonoBehaviour
{
    private Camera cam;
    private float xRotation = 0.0f;

    [Header("Sensitivity Settings")]
    public float xSensitivity = 10.0f;
    public float ySensitivity = 20.0f;
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInput;
    private float baseSensitivity = 1f;

    private float recoilX;
    private float recoilY;
    private float currentRecoilX;
    private float currentRecoilY;

    [Header("Recoil Settings")]
    public float recoilKickUp = 3f;
    public float recoilKickSide = 0.5f;
    public float recoilRecoverySpeed = 6f;

    [Header("Scope Settings")]
    public float normalFOV = 60f;
    public float scopeFOVSpeed = 10f;
    public Camera gunCamera;
    [HideInInspector] public bool isScoped = false;
    [HideInInspector] public float targetFOV;

    [Header("UI")]
    public GameObject settingsPanel;

    private GameManager gameManager;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        targetFOV = normalFOV;
        cam.fieldOfView = normalFOV;
        gameManager = FindAnyObjectByType<GameManager>();

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 0.1f;
            sensitivitySlider.maxValue = 3f;
            sensitivitySlider.value = baseSensitivity;
            sensitivitySlider.onValueChanged.AddListener(OnSliderChanged);
        }

        if (sensitivityInput != null)
        {
            sensitivityInput.text = baseSensitivity.ToString("F1");
            sensitivityInput.onEndEdit.AddListener(OnInputChanged);
        }
    }

    private void OnSliderChanged(float value)
    {
        baseSensitivity = value;
        if (sensitivityInput != null)
            sensitivityInput.text = value.ToString("F1");
    }

    private void OnInputChanged(string value)
    {
        float parsed;
        if (float.TryParse(value, out parsed))
        {
            parsed = Mathf.Clamp(parsed, sensitivitySlider.minValue, sensitivitySlider.maxValue);
            baseSensitivity = parsed;
            sensitivitySlider.value = parsed;
            sensitivityInput.text = parsed.ToString("F1");
        }
        else
        {
            sensitivityInput.text = baseSensitivity.ToString("F1");
        }
    }

    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, scopeFOVSpeed * Time.deltaTime);

        float recoilMult = isScoped ? 0.4f : 1f;

        currentRecoilX = Mathf.Lerp(currentRecoilX, recoilX, recoilRecoverySpeed * Time.deltaTime);
        currentRecoilY = Mathf.Lerp(currentRecoilY, recoilY, recoilRecoverySpeed * Time.deltaTime);

        recoilX = Mathf.Lerp(recoilX, 0f, recoilRecoverySpeed * Time.deltaTime);
        recoilY = Mathf.Lerp(recoilY, 0f, recoilRecoverySpeed * Time.deltaTime);

        float totalX = xRotation - (currentRecoilX * recoilMult);
        totalX = Mathf.Clamp(totalX, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(totalX, currentRecoilY * recoilMult, 0);
    }

    public void ToggleScope(float scopedFOV)
    {
        isScoped = !isScoped;
        targetFOV = isScoped ? scopedFOV : normalFOV;
        if (gunCamera != null)
            gunCamera.enabled = !isScoped;
    }

    public void ProcessLook(Vector2 input)
    {
        if (gameManager.isCursorVisible) return;

        float sensMultiplier = (isScoped ? 0.15f : 1f) * baseSensitivity;

        xRotation -= (input.y * Time.deltaTime) * ySensitivity * sensMultiplier;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.Rotate(Vector3.up * (input.x * Time.deltaTime) * xSensitivity * sensMultiplier);
    }

    public void AddRecoil()
    {
        recoilX += recoilKickUp;
        recoilY += Random.Range(-recoilKickSide, recoilKickSide);
    }
}