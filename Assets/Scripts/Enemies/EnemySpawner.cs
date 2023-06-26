using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnFrequency;
    private float nextSpawn;
    private float frequencyRange = 0.75f;
    public float chanceForExtraEnemy = 0;
    private int extraEnemyCount = 1;
    [SerializeField] private int directionModifier;
    [SerializeField] private bool addBox;
    [SerializeField] private GameObject initialEnemyModel;
    [SerializeField] private int initialEnemyWeight;
    [SerializeField] private AnimationCurve spawnProgression;
    [SerializeField] private AnimationCurve extraEnemyProgression;
    private Dictionary<GameObject, int> enemiesMatrix = new Dictionary<GameObject, int>();
    private List<GameObject> enemyPool = new List<GameObject>();
    private float timeCounter;

    private List<MutationBase> mutations = new List<MutationBase>();

    [HideInInspector] public bool active = false;


    private void Start()
    {
        enemiesMatrix.Add(initialEnemyModel, initialEnemyWeight);

        if(addBox) SpawnerManager.Main.SetEnemyBox(initialEnemyModel.GetComponent<EnemyManager>());
    }

    public void Activate()
    {
        active = true;
        SpawnEnemy();
        StartCoroutine(SpawnExtraEnemies());

        SetNextFrequency();
    }

    private void SetNextFrequency()
    {
        var min = frequencyRange + chanceForExtraEnemy;
        var max = frequencyRange - chanceForExtraEnemy;
        nextSpawn = Random.Range(spawnFrequency - min, spawnFrequency + max);
    }

    public void Deactivate()
    {
        active = false;
        timeCounter = 0;
    }

    public void RaiseParameters(int waveNumber, int totalWaves)
    {
        var perc = (float)waveNumber/totalWaves;
        spawnFrequency = spawnProgression.Evaluate(perc);
        extraEnemyCount = Mathf.RoundToInt(extraEnemyProgression.Evaluate(perc));
    }

    private void SpawnEnemy()
    {
        var list = GetWeightedEnemyList();
        var rdm = Random.Range(0, list.Count);
        var _enemy = list[rdm];

        var enemy = enemyPool.Find(x => x.name == _enemy.name);
        if (enemy == null) enemy = CreateNewEnemy(_enemy);
        else
        {
            enemyPool.Remove(enemy);
            enemy.SetActive(true);
        }

        enemy.GetComponent<EnemyHealthModule>().onEnemyDeath += AddEnemyToPool;
        enemy.GetComponent<EnemyMarchModule>().SetDirection(directionModifier);
        enemy.transform.position = transform.position;
        GeneralStatRegistry.Main.spawnedEnemies++;
        GeneralStatRegistry.Main.activeEnemies++;
    }

    private void AddEnemyToPool(EnemyHealthModule healthController, bool destroy)
    {
        healthController.onEnemyDeath -= AddEnemyToPool;
        if (destroy) Destroy(healthController.gameObject);
        else enemyPool.Add(healthController.gameObject);
        healthController.transform.position = transform.position;
        healthController.gameObject.SetActive(false);
    }

    private GameObject CreateNewEnemy(GameObject enemy)
    {
        var baby = Instantiate(enemy, transform.position, enemy.transform.rotation);
        baby.name = enemy.name;
        return baby;
    }

    private List<GameObject> GetWeightedEnemyList()
    {
        List<GameObject> list = new List<GameObject>();

        foreach (var enemy in enemiesMatrix.Keys)
        {
            for (int i = 0; i < enemiesMatrix[enemy]; i++)
            {
                list.Add(enemy);
            }
        }

        return list;
    }

    private void Update()
    {
        if (!active) return;

        timeCounter += Time.deltaTime;

        if(timeCounter >= nextSpawn)
        {
            SpawnEnemy();
            StartCoroutine(SpawnExtraEnemies());

            timeCounter = 0;
            SetNextFrequency();
        }

    }

    private IEnumerator SpawnExtraEnemies()
    {
        var rdm = Random.Range(0, 1f);
        
        if(rdm <= chanceForExtraEnemy)
        {
            var count = Random.Range(0, extraEnemyCount + 1);

            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(1f);
                SpawnEnemy();
            }
        }


    }

    public void ReceiveEnemy(GameObject enemy)
    {
        if(enemiesMatrix.ContainsKey(enemy))
        {
            enemiesMatrix[enemy]++;
        } else
        {
            enemiesMatrix.Add(enemy, 1);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemiesMatrix.ContainsKey(enemy))
        {
            enemiesMatrix[enemy]--;

            if (enemiesMatrix[enemy] <= 0)
            {
                enemiesMatrix.Remove(enemy);
            }
        }
    }

    public List<GameObject> GetEnemiesInPool()
    {
        return enemyPool;
    }

    public void ReceiveMutation(MutationBase mutation)
    {
        var mut = mutations.Find(x => x.Compare(mutation));
        if (mut != null) mut.AddMutation(mutation);
        else
        {
            var _m = mutation.GetCopy();
            mutations.Add(_m);
        }
        
    }
}