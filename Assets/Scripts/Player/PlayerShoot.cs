using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerShoot: MonoBehaviour
{

    private float shotTimer;
    
    [Range(0.1f,2)]
    public float fireRate;

    public Transform playerGunBarrelTransform;
    private Ray ray;
    public float bulletVelocity = 200;
    private AudioSource[] playerSounds;
    private AudioSource gunShotSound;
    private AudioSource reloadSound;
    
    public int ammoCounter;
    public int gunAmmo = 30;
    private bool isReloading;
    
    // counter to count the time of reload during the animation of reload so player cant shoot
    public float reloadCooldownCounter;
    private float reloadTime = 3.0f;

    public TextMeshProUGUI reloadText;

    public ParticleSystem muzzleFlash;

    void Start()
    {
        ammoCounter = gunAmmo;
        playerSounds = GetComponents<AudioSource>();
        gunShotSound = playerSounds[0];
        reloadSound = playerSounds[2];
    }
    void Update()
    {
        reloadText.text ="Ammo: " + ammoCounter.ToString() + "/" + gunAmmo.ToString();
        ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0)
        );
        shotTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && shotTimer > fireRate && ammoCounter > 0 && !isReloading)
        {
            Shoot();
        }
        if (ammoCounter <= 0 && !isReloading)
        {
            ReloadGun();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadGun();
        }
        if (isReloading)
        {
            reloadCooldownCounter += Time.deltaTime;
        }
        if (reloadCooldownCounter > reloadTime)
        {   
            isReloading = false;
            ammoCounter = gunAmmo;
            reloadCooldownCounter = 0;
        }
    }

    void Shoot()
    {

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f;
        }
        ammoCounter -= 1;
        shotTimer = 0;
        Debug.Log("Player shotted");
        gunShotSound.Play();
        muzzleFlash.Play();
        Transform gunBarrel = playerGunBarrelTransform;
        // instantiate the bullet object at that position of gun barrel
        Vector3 shootDir = (targetPoint - playerGunBarrelTransform.position).normalized;
        GameObject bullet = GameObject.Instantiate(Resources.Load("prefabs/Bullet") as GameObject
        ,gunBarrel.position,Quaternion.LookRotation(shootDir) * Quaternion.Euler(90,0,0));
        bullet.GetComponent<Rigidbody>().linearVelocity = shootDir * bulletVelocity;

    }

    public void ReloadGun()
    {
        isReloading = true;
        reloadSound.Play();
        
        // play an animation of reloading
    }
}