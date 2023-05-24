using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnerManager : MonoBehaviour
{
    #region Main
    private static SpawnerManager _instance;
    public static SpawnerManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SpawnerManager>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private List<EnemyAddData> adds;
    [SerializeField] private Database database;
    [SerializeField] private int maxWaves;
    private int waveNumber = 1;
    [SerializeField] private List<float> waveDurations;
    private float currentDuration;
    private float counter;
    [SerializeField] private GameObject startButton;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI waveIndicator, playlist, waveTime, upperText;
    private List<EnemySpawner> spawners;
    private bool preWaveEnd = true;
    public bool OnWave {get; private set;}
    private bool paused;

    [Space]
    [SerializeField] private EnemyBox boxModel;
    [SerializeField] private Transform enemyPanel;
    [SerializeField] private TextMeshProUGUI enemyVolume;


    public delegate void EndOfWaveEvent(int waveNumber);
    public EndOfWaveEvent OnEndOfWave;



    private void Start()
    {
        currentDuration = waveDurations[waveNumber -1];
        playlist.text = waveNumber + "/" + (maxWaves);
        spawners = FindObjectsOfType<EnemySpawner>().ToList();
        waveIndicator.text = "WAVE " +  waveNumber;
    }

    public void SetEnemyBox(EnemyManager enemy)
    {
        var box = Instantiate(boxModel, enemyPanel);
        box.ReceiveEnemy(enemy);
    }

    public void InitiateWave()
    {
        if(!OnWave)
        {
            OnWave = true;
            preWaveEnd = false;
            counter = 0;
            spawners.ForEach(x => x.Activate());
            startButton.SetActive(false);
            upperText.text = "Now playing:";

            ActionMarker.Main.On = true;
        }
        else
        {
            Time.timeScale = 1;
            paused = false;
            startButton.SetActive(false);
            AudioManager.Main.UnpauseAudio();
        }
    }

    public void SetEnemyChance(float value)
    {
        spawners.ForEach(x => x.chanceForExtraEnemy = value);
        enemyVolume.text = (value * 100).ToString("0") + "%";
    }

    public void Pause()
    {
        Time.timeScale = 0;
        paused = true;
        startButton.SetActive(true);
        AudioManager.Main.PauseAudio();
    }

    public IEnumerator EndWave()
    {
        preWaveEnd = true;
        waveNumber++;

        spawners.ForEach(x =>
        {
            x.Deactivate();
            x.RaiseParameters(waveNumber, maxWaves - 1);
        });

        yield return new WaitUntil(() => FindObjectsOfType<EnemyManager>().Length == 0);

        OnWave = false;
        ActionMarker.Main.On = false;
        ActionMarker.Main.Reset();

        foreach (var weapon in FindObjectsOfType<WeaponBase>())
        {
            weapon.Stop();
        }

        if (waveNumber == maxWaves + 1) 
        {
            EndGameLog.Main.TriggerEndgame(true);
            yield break;
        }

        startButton.SetActive(true);

        currentDuration = waveDurations[waveNumber - 1];

        OnEndOfWave?.Invoke(waveNumber);

        ProcessNewAdds();

        playlist.text = waveNumber + "/" + (maxWaves);
        progressBar.value = 0;
        waveTime.text = "00:00/" + GetTimeInMinutes(currentDuration);
        upperText.text = "Next:";
        waveIndicator.text = "WAVE " +  waveNumber;
    }

    private void ProcessNewAdds()
    {
        foreach(var add in adds)
        {
            if(waveNumber == add.requiredWave)
            {
                foreach (var _spw in add.spawnersToUnlock)
                {
                    _spw.gameObject.SetActive(true);
                    spawners.Add(_spw);
                }

                foreach(var spw in add.spawnersToApply)
                {
                    foreach(var enemy in add.enemies)
                    {
                        spw.ReceiveEnemy(enemy);

                        if(add.addBox)
                        {
                            SetEnemyBox(enemy.GetComponent<EnemyManager>());
                        }
                    }
                }
            }
        }
    }

    private string GetTimeInMinutes(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60f);
        var seconds = time - (minutes * 60);

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void Update()
    {
        if (!preWaveEnd)
        {
            counter += Time.deltaTime;

            progressBar.value = counter / currentDuration;
            waveTime.text = GetTimeInMinutes(counter) + "/" + GetTimeInMinutes(currentDuration);

            if (counter >= currentDuration)
            {
                StartCoroutine(EndWave());
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!OnWave) InitiateWave();
            else
            {
                if(paused) InitiateWave();
                else Pause();
            }
        }
    }
}

[System.Serializable]
public struct EnemyAddData
{
    public int requiredWave;
    public bool addBox;
    public List<EnemySpawner> spawnersToApply;
    public List<EnemySpawner> spawnersToUnlock;
    public List<GameObject> enemies;
}