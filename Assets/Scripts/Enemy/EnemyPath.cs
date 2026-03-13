using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath: MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    public static implicit operator EnemyPath(GameObject v)
    {
        throw new NotImplementedException();
    }
}