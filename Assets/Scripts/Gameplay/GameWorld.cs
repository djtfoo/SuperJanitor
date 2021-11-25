using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    [SerializeField]
    private BossEnemy bossEnemy;
    public BossEnemy BossEnemy
    {
        get { return bossEnemy; }
    }

    [SerializeField]
    private Transform[] bossWaypoints;
    public Transform[] BossWaypoints
    {
        get { return bossWaypoints; }
    }
}
