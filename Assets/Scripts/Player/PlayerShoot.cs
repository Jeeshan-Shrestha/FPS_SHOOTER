using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    private float shotTimer;
    private Ray ray;
    public float bulletVelocity = 200;

    public int ammoCounter;
    public BaseGun gun;
    private bool isReloading;

    public float reloadCooldownCounter;
    private float reloadTime = 3.0f;

    public TextMeshProUGUI reloadText;
    public ParticleSystem muzzleFlash;

    [Header("Scope UI")]
        // assign a full-screen scope UI image in Inspector
    public GameObject crosshairUI;     // assign your crosshair UI object in Inspector

    private PlayerLook playerLook;

    public SettingsMenu settingsMenu;
    public ShopMenu shopMenu;


    [SerializeField]private GameObject sniper;
    [SerializeField]private GameObject assaultRifle;

    private GameObject bulletPrefab;
    private GameObject sniperBulletPrefab;

    private GameObject bullet;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        bulletPrefab = Resources.Load("prefabs/Bullet") as GameObject;
        sniperBulletPrefab = Resources.Load("prefabs/SniperBullet") as GameObject;
        sniper.SetActive(false);
        assaultRifle.SetActive(true);
        gun = GetComponentInChildren<BaseGun>();
        bulletVelocity = gun.bulletVelocity;
        ammoCounter = gun.maxAmmo;
        playerLook = GetComponentInParent<PlayerLook>();

        if (gun.scopeOverlay) gun.scopeOverlay.SetActive(false);
        if (crosshairUI) crosshairUI.SetActive(true);
    }

    void Update()
    {
        reloadText.text = "Ammo: " + ammoCounter.ToString() + "/" + gun.maxAmmo.ToString();
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        shotTimer += Time.deltaTime;

        // Sync scope UI
        if (gun.scopeOverlay) gun.scopeOverlay.SetActive(playerLook.isScoped);
        if (crosshairUI) crosshairUI.SetActive(!playerLook.isScoped);

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll < 0f && ShopManager.instance.sniperUnlocked)
            SwapGun(sniper, assaultRifle);

        if (scroll > 0f)
            SwapGun(assaultRifle, sniper);


        if (Input.GetMouseButton(0) 
        && shotTimer > gun.fireRate 
        && ammoCounter > 0 
        && !isReloading 
        && !settingsMenu.isPanelOpen
        && !shopMenu.isPanelOpen
        && !gameManager.isCursorVisible
        )
            Shoot();

        if (ammoCounter <= 0 && !isReloading)
            ReloadGun();

        if (Input.GetKeyDown(KeyCode.R))
            ReloadGun();

        if (isReloading)
            reloadCooldownCounter += Time.deltaTime;

        if (Input.GetMouseButtonDown(1) 
            && !settingsMenu.isPanelOpen 
            && !shopMenu.isPanelOpen
            && !gameManager.isCursorVisible
            )
        {
            playerLook.ToggleScope(gun.scopedFOV);
        }

        if (reloadCooldownCounter > reloadTime)
        {
            isReloading = false;
            ammoCounter = gun.maxAmmo;
            reloadCooldownCounter = 0;
        }
    }

    void Shoot()
    {
        Debug.Log(gun.gunName);
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 100f))
            targetPoint = hit.point;
        else
            targetPoint = ray.origin + ray.direction * 100f;

        ammoCounter -= 1;
        shotTimer = 0;
        gun.gunShotSound.Play();
        muzzleFlash.Play();
        playerLook.AddRecoil();

        Vector3 shootDir = (targetPoint - gun.gunBarrelPos.position).normalized;
        if (gun.gunName == "AssaultRifle")
        {
            bullet = Instantiate(
            Resources.Load("prefabs/Bullet") as GameObject,
            gun.gunBarrelPos.position,
            Quaternion.LookRotation(shootDir) * Quaternion.Euler(90, 0, 0)
        );
        }
        if (gun.gunName == "Sniper")
        {
            bullet = Instantiate(
            Resources.Load("prefabs/SniperBullet") as GameObject,
            gun.gunBarrelPos.position,
            Quaternion.LookRotation(shootDir) * Quaternion.Euler(90, 0, 0));
        }
        
        bullet.GetComponent<Rigidbody>().linearVelocity = shootDir * bulletVelocity;
    }

    public void ReloadGun()
    {
        isReloading = true;
        gun.reloadSound.Play();
    }

    void SwapGun(GameObject activate, GameObject deactivate)
    {
        deactivate.SetActive(false);
        activate.SetActive(true);

        gun = activate.GetComponent<BaseGun>();
        ammoCounter = gun.maxAmmo;
        muzzleFlash = gun.GetComponentInChildren<ParticleSystem>();
        bulletVelocity = gun.bulletVelocity;
        
        if (playerLook.isScoped)
        {
            playerLook.isScoped = false;
            playerLook.targetFOV = playerLook.normalFOV;
            if (playerLook.gunCamera != null)
                playerLook.gunCamera.enabled = true;
        }

        if (gun.scopeOverlay) gun.scopeOverlay.SetActive(false);
        if (crosshairUI) crosshairUI.SetActive(true);

        isReloading = false;
        reloadCooldownCounter = 0;
        shotTimer = 0;
    }
}