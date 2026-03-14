using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    public float fireRate;
    public int maxAmmo;
    public float ReloadTimer;
    public AudioSource gunShotSound;
    public AudioSource reloadSound;
    public Transform gunBarrelPos;
    public GameObject scopeOverlay;
}
