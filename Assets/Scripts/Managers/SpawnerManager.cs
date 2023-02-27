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
    private bool OnWave;

    [Space]
    [SerializeField] private GameObject upgradeButton;

    public delegate void EndOfWaveEvent(int waveNumber);
    public EndOfWaveEvent OnEndOfWave;



    private void Start()
    {
        playlist.text = waveNumber + "/" + (maxWaves - 1);
        spawners = FindObjectsOfType<EnemySpawner>().ToList();
        waveIndicator.text = "WAVE " +  waveNumber;
    }

    public void InitiateWave()
    {
        if(!OnWave)
        {
            OnWave = true;
            counter = 0;
            spawners.ForEach(x => x.Activate());
            startButton.SetActive(false);
            upperText.text = "Now playing:";

            ActionMarker.Main.On = true;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public IEnumerator EndWave()
    {
        OnWave = false;
        waveNumber++;

        spawners.ForEach(x =>
        {
            x.Deactivate();
            x.RaiseParameters(waveNumber, maxWaves - 1);
        });

        yield return new WaitUntil(() => FindObjectsOfType<EnemyManager>().Length == 0);

        currentDuration = waveDurations[waveNumber];
        if (waveNumber == maxWaves) EndGameLog.Main.TriggerEndgame(true);
        else startButton.SetActive(true);

        ActionMarker.Main.On = false;
        ActionMarker.Main.Reset();

        if (upgradeButton.activeSelf == false)
        {
            upgradeButton.SetActive(true);
            TutorialManager.Main.RequestTutorialPage(11, 2);
        }

        foreach (var weapon in FindObjectsOfType<WeaponBase>())
        {
            weapon.Stop();
        }

        OnEndOfWave?.Invoke(waveNumber);

        ProcessNewAdds();

        playlist.text = waveNumber + "/" + (maxWaves - 1);
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
                foreach(var spw in add.spawners)
                {
                    add.enemies.ForEach(x => spw.ReceiveEnemy(x));
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
        if (OnWave)
        {
            counter += Time.deltaTime;

            progressBar.value = counter / currentDuration;
            waveTime.text = GetTimeInMinutes(counter) + "/" + GetTimeInMinutes(currentDuration);

            if (counter >= currentDuration)
            {
                StartCoroutine(EndWave());
            }
        }
    }
}

[System.Serializable]
public struct EnemyAddData
{
    public int requiredWave;
    public List<EnemySpawner> spawners;
    public List<GameObject> enemies;
}