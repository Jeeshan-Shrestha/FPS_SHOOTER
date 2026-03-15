using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    private float bulletSpeed = 250f;

    private int currentAmmo;
    private float reloadTimer;
    private bool isReloading;

    public override void Enter()
    {
        currentAmmo = enemy.maxAmmo;
        isReloading = false;
        reloadTimer = 0;
    }

    public override void Exit()
    {
        isReloading = false;
        reloadTimer = 0;
    }

    public override void Process()
    {
        // handle reload timer
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= enemy.reloadTime)
            {
                isReloading = false;
                reloadTimer = 0;
                currentAmmo = enemy.maxAmmo;
                Debug.Log("Enemy reloaded");
            }
            return; // do nothing while reloading
        }

        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);

            if (shotTimer > enemy.fireRate)
            {
                if (currentAmmo > 0)
                    Shoot();
                else
                    StartReload();
            }

            if (moveTimer > Random.Range(2, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 5)
                stateMachine.ChangeState(new PatrolState());
        }
    }

    void StartReload()
    {
        isReloading = true;
        reloadTimer = 0;
        enemy.reloadSound.Play();
        Debug.Log("Enemy reloading...");
    }

    public void Shoot()
{
    shotTimer = 0;
    currentAmmo--; 
    enemy.gunShotSound.Play();
    enemy.muzzleFlash.Play();

    Transform gunBarrel = enemy.gunBarrelTransform;
    Vector3 shootDir = (enemy.Player.transform.position - gunBarrel.position).normalized;

    // add random spread
    float spread = enemy.bulletSpread;
    shootDir += new Vector3(
        Random.Range(-spread, spread),
        Random.Range(-spread, spread),
        Random.Range(-spread, spread)
    );
    shootDir.Normalize();

    GameObject prefab = enemy.bulletPrefab != null
        ? enemy.bulletPrefab
        : Resources.Load("prefabs/Bullet") as GameObject;

    GameObject bullet = GameObject.Instantiate(
        prefab,
        gunBarrel.position,
        Quaternion.LookRotation(shootDir)
    );

    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), enemy.GetComponent<Collider>());
    bullet.GetComponent<Rigidbody>().linearVelocity = shootDir * bulletSpeed;
}
}