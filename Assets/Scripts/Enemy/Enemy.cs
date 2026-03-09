using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;

    public NavMeshAgent Agent {get => agent;}

    [SerializeField]
    private String currentState;

    public EnemyPath enemyPath;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();
        stateMachine.Initialize();
    }
}