using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStatRegistry : MonoBehaviour
{
    #region Singleton
    private static GeneralStatRegistry _instance;
    public static GeneralStatRegistry Main
    {
        get
        {
            if(_instance == null) _instance = FindObjectOfType<GeneralStatRegistry>();

            return _instance;
        }
    }

    #endregion
    public int totalDamageDealt;
    public int totalDamageTaken;
    public int currentCash;
    public int currentWave;
    public int spawnedEnemies;
    public int activeEnemies;
    public int totalWeapons;
    public int totalTiles;

    void Update()
    {
        currentCash = CrystalManager.Main.buildPoints;
        totalDamageTaken = EndGameLog.Main.damageTaken;
    }
}
