using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    public string gunName;
    public float fireRate;
    public int maxAmmo;
    public float ReloadTimer;
    public AudioSource gunShotSound;
    public AudioSource reloadSound;
    public Transform gunBarrelPos;
    public GameObject scopeOverlay;
    public float gunDamage;
    public float scopedFOV = 30f;
    public float bulletVelocity;
}
