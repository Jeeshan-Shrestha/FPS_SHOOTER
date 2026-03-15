using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null) return;
    transform.LookAt(cam.transform.position);
    transform.Rotate(0, 180, 0);
    }
}