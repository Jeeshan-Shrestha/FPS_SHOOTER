using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Process()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public void PatrolCycle()
    {
        if (!enemy.Agent.isOnNavMesh || !enemy.Agent.isActiveAndEnabled)
        return;
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 1f)
            {
                if (waypointIndex < enemy.enemyPath.waypoints.Count - 1)
            {
                waypointIndex++;
            }
            else
            {
                waypointIndex = 0;
            }
            enemy.Agent.SetDestination(enemy.enemyPath.waypoints[waypointIndex].position);
            waitTimer = 0f;
            }
            
        }
        
    }
}