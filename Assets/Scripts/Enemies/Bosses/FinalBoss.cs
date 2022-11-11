using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private EnemyHealthController healthController;
    private static int bossesDefeated;

    private void Awake()
    {
        healthController = GetComponent<EnemyHealthController>();
        healthController.onEnemyDeath += BossDeath;
    }

    private void BossDeath(EnemyHealthController healthController, bool destroy)
    {
        Destroy(gameObject);
        bossesDefeated++;

        if(bossesDefeated == 2) EndGameLog.Main.TriggerEndgame(true);
    }
}
