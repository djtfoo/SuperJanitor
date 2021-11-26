using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLivesDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("First array element should be the leftmost")]
    private GameObject[] enemyLivesDisplay;

    private BossEnemy bossEnemy = null;

    // Start is called before the first frame update
    void Start()
    {
        // initially disable
        foreach (GameObject obj in enemyLivesDisplay)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bossEnemy == null)
        {
            bossEnemy = (BossEnemy)GameObject.FindObjectOfType<BossEnemy>();
        }
        else
        {
            int livesRemaining = bossEnemy.MaxHitsAllowed - bossEnemy.HitsTaken;
            for (int i = 0; i < bossEnemy.MaxHitsAllowed; ++i)
            {
                if (!bossEnemy.FirstWaveCompleted)
                    enemyLivesDisplay[i].SetActive(false);
                else
                    enemyLivesDisplay[i].SetActive(i < livesRemaining);
            }
        }
    }
}
