using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    private float bulletSpeed = 250f;

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Process()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);

            if (shotTimer > enemy.fireRate)
                Shoot();

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

    public void Shoot()
    {
        shotTimer = 0;
        enemy.gunShotSound.Play();
        enemy.muzzleFlash.Play();

        Transform gunBarrel = enemy.gunBarrelTransform;
        Vector3 shootDir = (enemy.Player.transform.position - gunBarrel.position).normalized;

        GameObject prefab = enemy.bulletPrefab != null
            ? enemy.bulletPrefab
            : Resources.Load("prefabs/Bullet") as GameObject;  // fallback

        GameObject bullet = GameObject.Instantiate(
            prefab,
            gunBarrel.position,
            Quaternion.LookRotation(shootDir)
        );

        bullet.GetComponent<Rigidbody>().linearVelocity = shootDir * bulletSpeed;
    }
}