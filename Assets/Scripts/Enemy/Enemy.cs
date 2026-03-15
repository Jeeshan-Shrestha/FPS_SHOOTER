using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public AudioSource gunShotSound;
    public ParticleSystem muzzleFlash;

    [Range(0f, 0.3f)]
    public float bulletSpread = 0.05f;

    public NavMeshAgent Agent { get => agent; }

    [SerializeField]
    private String currentState;

    public EnemyPath enemyPath;

    private GameObject player;
    public GameObject Player { get => player; }
    public float sightDistance = 100f;
    public float fieldOfView = 80f;

    public Transform gunBarrelTransform;

    [Range(0.1f, 10f)]
    public float fireRate;

    public GameObject bulletPrefab;

    public int maxAmmo = 10;
    public float reloadTime = 2.5f;

    private AudioSource[] sounds;
    public AudioSource reloadSound;

   void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");

        sounds = GetComponents<AudioSource>();

        if (sounds.Length < 2)
        {
            Debug.LogError("Enemy needs at least 2 AudioSource components!");
            return;
        }

        gunShotSound = sounds[0];
        reloadSound = sounds[1];
    }

    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public void OnHit()
    {
        // face player immediately
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        dirToPlayer.y = 0;
        transform.rotation = Quaternion.LookRotation(dirToPlayer);

        // force switch to attack state
        stateMachine.ChangeState(new AttackState());
    }

    public bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
            if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(transform.position, targetDirection);
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(ray, out hitInfo, sightDistance))
                {
                    if (hitInfo.transform.gameObject == player)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                        return true;
                    }
                }
            }
        }
        return false;
    }

}