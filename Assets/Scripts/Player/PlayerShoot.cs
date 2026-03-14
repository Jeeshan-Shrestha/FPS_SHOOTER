using TMPro;
using UnityEngine;

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

    void Start()
    {
        gun = GetComponentInChildren<BaseGun>();
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

        if (Input.GetMouseButton(0) && shotTimer > gun.fireRate && ammoCounter > 0 && !isReloading && !settingsMenu.isPanelOpen)
            Shoot();

        if (ammoCounter <= 0 && !isReloading)
            ReloadGun();

        if (Input.GetKeyDown(KeyCode.R))
            ReloadGun();

        if (isReloading)
            reloadCooldownCounter += Time.deltaTime;

        if (reloadCooldownCounter > reloadTime)
        {
            isReloading = false;
            ammoCounter = gun.maxAmmo;
            reloadCooldownCounter = 0;
        }
    }

    void Shoot()
    {
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
        GameObject bullet = Instantiate(
            Resources.Load("prefabs/Bullet") as GameObject,
            gun.gunBarrelPos.position,
            Quaternion.LookRotation(shootDir) * Quaternion.Euler(90, 0, 0)
        );
        bullet.GetComponent<Rigidbody>().linearVelocity = shootDir * bulletVelocity;
    }

    public void ReloadGun()
    {
        isReloading = true;
        gun.reloadSound.Play();
    }
}