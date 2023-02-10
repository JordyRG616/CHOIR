using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnFrequency;
    public float chanceForExtraEnemy = 0;
    [SerializeField] private int directionModifier;
    [SerializeField] private GameObject initialEnemyModel;
    [SerializeField] private int initialEnemyWeight;
    private Dictionary<GameObject, int> enemiesMatrix = new Dictionary<GameObject, int>();
    private List<GameObject> enemyPool = new List<GameObject>();
    [field: SerializeField] public SpawnerInfo infoUI { get; private set; }
    private float timeCounter;

    private List<MutationBase> mutations = new List<MutationBase>();

    public enum SpawnerPosition { Left, Right, Both, None}
    public SpawnerPosition position;

    [HideInInspector] public bool active = false;


    private void Start()
    {
        enemiesMatrix.Add(initialEnemyModel, initialEnemyWeight);
    }

    public void Activate()
    {
        active = true;
        SpawnEnemy();
    }

    public void Deactivate()
    {
        active = false;
        timeCounter = 0;
    }

    public void RaiseParameters(int tankLevel)
    {
        spawnFrequency -= tankLevel / 10;
        chanceForExtraEnemy += tankLevel / 10;
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
        enemy.transform.localScale = new Vector3
            (Mathf.Abs(enemy.transform.localScale.x) * directionModifier,
            enemy.transform.localScale.y, 1);
        ApplyMutations(enemy);
    }

    private void ApplyMutations(GameObject enemy)
    {
        var manager = enemy.GetComponent<EnemyManager>();

        manager.SetStats();

        mutations.ForEach(x => x.ApplyMutation(manager.currentStats));

        manager.SetStats(false);
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

    public int GetTotal()
    {
        int total = 0;

        foreach (var enemy in enemiesMatrix.Keys)
        {
            total += enemiesMatrix[enemy];
        }

        return total;
    }

    public List<(string enemyName, float percentage)> GetEnemiesPercentages()
    {
        var list = new List<(string enemyName, float percentage)>();

        foreach (var enemy in enemiesMatrix.Keys)
        {
            (string name, float value) t;
            t.name = enemy.name;
            t.value = (float)enemiesMatrix[enemy] / GetTotal();
            list.Add(t);
        }

        return list;
    }

    private void Update()
    {
        if (!active) return;

        timeCounter += Time.deltaTime;

        if(timeCounter >= spawnFrequency)
        {
            SpawnEnemy();
            StartCoroutine(SpawnExtraEnemies());
            timeCounter = 0;
        }

    }

    private IEnumerator SpawnExtraEnemies()
    {
        var rdm = Random.Range(0, chanceForExtraEnemy);
        var count = Mathf.RoundToInt(rdm);

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(.66f);
            SpawnEnemy();
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

        //infoUI.SetEnemieInfo(GetEnemiesPercentages());
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

        //infoUI.SetEnemieInfo(GetEnemiesPercentages());
    }

    public List<GameObject> GetEnemiesInPool()
    {
        return enemyPool;
    }

    public void LowerFrequency(float value)
    {
        spawnFrequency -= value;
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