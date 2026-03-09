using UnityEngine;

public class PatrolState : BaseState
{

    public int waypointIndex;
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Process()
    {
        PatrolCycle();
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
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
        }
    }
}