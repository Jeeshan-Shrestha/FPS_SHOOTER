using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public AudioSource gunShotSound;
    public ParticleSystem muzzleFlash;

    public NavMeshAgent Agent {get => agent;}

    [SerializeField]
    private String currentState;

    public EnemyPath enemyPath;

    private GameObject player;
    public GameObject Player { get => player;}
    public float sightDistance = 30f;
    public float fieldOfView = 80f;

    public Transform gunBarrelTransform;

    [Range(0.1f,10f)]
    public float fireRate;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");
        gunShotSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position,player.transform.position) < sightDistance)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(targetDirection,transform.forward);
            if (angleToPlayer >= -fieldOfView &&angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(transform.position,targetDirection);
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(ray,out hitInfo, sightDistance))
                {
                    if (hitInfo.transform.gameObject == player)
                    {
                        Debug.DrawRay(ray.origin,ray.direction * sightDistance, Color.red);
                        return true;
                    }
                }
                
            }
        }
        return false;
    }
}