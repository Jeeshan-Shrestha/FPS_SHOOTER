using UnityEngine;

public class AttackState : BaseState
{

    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;

    public float bulletSpeed = 40;    
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
            {
                Shoot();
            }
            if (moveTimer  > Random.Range(2, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 5)
            {
                // change to search state
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public void Shoot()
    {
        shotTimer = 0;
        Debug.Log("Shoot");

        // take the reference to the gun barrel
        Transform gunBarrel = enemy.gunBarrelTransform;
        // instantiate the bullet object at that position of gun barrel
        GameObject bullet = GameObject.Instantiate(Resources.Load("prefabs/Bullet") as GameObject,gunBarrel.position,enemy.transform.rotation);
        // calculate the direction of the bullet to that of the player
        Vector3 shootDir = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;
        // add force to the bullet 
        bullet.GetComponent<Rigidbody>().linearVelocity = shootDir * bulletSpeed;
    }
}
