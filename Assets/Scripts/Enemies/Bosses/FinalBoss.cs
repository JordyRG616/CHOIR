using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private EnemyHealthModule healthController;
    private static int bossesDefeated;

    private void Awake()
    {
        healthController = GetComponent<EnemyHealthModule>();
        healthController.onEnemyDeath += BossDeath;
    }

    private void BossDeath(EnemyHealthModule healthController, bool destroy)
    {
        Destroy(gameObject);
        bossesDefeated++;

        if(bossesDefeated == 2) EndGameLog.Main.TriggerEndgame(true);
    }
}
