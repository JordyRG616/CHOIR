using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [SerializeField] private Database database;
    [SerializeField] private int maxWaves;
    private int waveNumber;
    [SerializeField] private List<float> waveDurations;
    private float currentDuration;
    private float counter;
    [SerializeField] private GameObject startButton;
    [SerializeField] private RectTransform fill;
    [SerializeField] private TMPro.TextMeshProUGUI waveIndicator;
    private List<EnemySpawner> spawners;
    private bool OnWave;

    [Space]
    [SerializeField] private GameObject upgradeButton;

    private void Start()
    {
        waveIndicator.text = "wave " + waveNumber + "/" + (maxWaves - 1);
        spawners = FindObjectsOfType<EnemySpawner>().ToList();
    }

    public void InitiateWave()
    {
        currentDuration = waveDurations[waveNumber];
        OnWave = true;
        counter = 0;
        spawners.ForEach(x => x.Activate());
        startButton.SetActive(false);

        ActionMarker.Main.On = true;
    }

    public void PassMutation(MutationBase mutation)
    {
        spawners.ForEach(x => x.ReceiveMutation(mutation));
    }

    public IEnumerator EndWave()
    {
        OnWave = false;
        waveNumber++;

        spawners.ForEach(x =>
        {
            x.Deactivate();
            x.RaiseParameters(waveNumber);
        });

        yield return new WaitUntil(() => FindObjectsOfType<EnemyManager>().Length == 0);

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

        var mutation = database.GetRandomMutation();
        PassMutation(mutation);
        waveIndicator.text = "wave " + waveNumber + "/" + (maxWaves - 1);
    }

    private void Update()
    {
        if (OnWave)
        {
            counter += Time.deltaTime;

            fill.sizeDelta = Vector2.Lerp(new Vector2(0, 25), new Vector2(140, 25), counter / currentDuration);

            if (counter >= currentDuration)
            {
                StartCoroutine(EndWave());
            }
        }
    }
}
