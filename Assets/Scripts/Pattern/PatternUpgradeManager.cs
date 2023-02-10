using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PatternUpgradeManager : MonoBehaviour
{
    #region Main
    private static PatternUpgradeManager _instance;
    public static PatternUpgradeManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<PatternUpgradeManager>();

            return _instance;
        }

    }
    #endregion

    public List<GameObject> upgradeCards;
    private List<GameObject> upgradesOnCooldown = new List<GameObject>();
    [HideInInspector] public float expMultiplier = 1;
    public TextMeshProUGUI multiplierValue;
    public GameObject upgradePanel;
    private GameObject firstCard, secondCard;

    public List<EnemySpawner> spawners;

    [Header("Add Enemy Upgrades")]
    public List<AddEnemyToSpawner> adds;
    private AddEnemyToSpawner leftAdd, rightAdd;
    [SerializeField] private int addIndex = 2;

    [Header("Additional Rows")]
    public List<GameObject> thirdRows;
    public List<GameObject> fourthRows;
    private int rowsAdded;

    [Header("Additional Lines")]
    public GameObject secondLine;
    public GameObject thirdLine;
    public GameObject fourthLine;
    [Space]
    public RectTransform patternBox;
    public float increment;
    private int linesAdded;

    private bool doTutorial = true;

    private void Start()
    {
        multiplierValue.text = "x" + expMultiplier.ToString() + " EXP";
    }

    public void InitiateSelection()
    {
        AudioManager.Main.PauseAudio();
        addIndex++;
        if (addIndex > adds.Count) addIndex = adds.Count;

        spawners.ForEach(x => x.chanceForExtraEnemy += .5f);
        expMultiplier += .1f;
        multiplierValue.text = "EXP x" + expMultiplier.ToString();

        if (upgradeCards.Count < 2)
        {
            upgradeCards.AddRange(upgradesOnCooldown);
            upgradesOnCooldown.Clear();
        }

        var rdm = Random.Range(0, upgradeCards.Count);
        firstCard = upgradeCards[rdm];
        upgradeCards.Remove(firstCard);
        rdm = Random.Range(0, upgradeCards.Count);
        secondCard = upgradeCards[rdm];
        upgradeCards.Remove(secondCard);

        upgradesOnCooldown.Add(firstCard);
        upgradesOnCooldown.Add(secondCard);

        firstCard.SetActive(true);
        secondCard.transform.SetAsLastSibling();
        secondCard.SetActive(true);
        upgradePanel.SetActive(true);

        SetAdds();
    }

    private void SetAdds()
    {
        var rdm = Random.Range(0, addIndex);
        leftAdd = adds[rdm];
        var firstTrigger = firstCard.GetComponent<EventTrigger>();
            
        firstTrigger.triggers[2].callback.AddListener((data) => PreviewEnemyAdd(EnemySpawner.SpawnerPosition.Left));
        firstTrigger.triggers[3].callback.AddListener((data) => UndoPreview(EnemySpawner.SpawnerPosition.Left));

        rdm = Random.Range(0, addIndex);
        rightAdd = adds[rdm];
        var secondTrigger = secondCard.GetComponent<EventTrigger>();

        secondTrigger.triggers[2].callback.AddListener((data) => PreviewEnemyAdd(EnemySpawner.SpawnerPosition.Right));
        secondTrigger.triggers[3].callback.AddListener((data) => UndoPreview(EnemySpawner.SpawnerPosition.Right));
    }

    public void AddEnemyToSpawner(EnemySpawner.SpawnerPosition position)
    {
        var spawner = spawners.Find(x => x.position == position);

        if (position == EnemySpawner.SpawnerPosition.Left) leftAdd.ApplyUpgrade(spawner);
        else rightAdd.ApplyUpgrade(spawner);
    }

    public void PreviewEnemyAdd(EnemySpawner.SpawnerPosition position)
    {
        var spawner = spawners.Find(x => x.position == position);

        spawner.infoUI.highlight.Play();

        if (position == EnemySpawner.SpawnerPosition.Left) spawner.ReceiveEnemy(leftAdd.enemy);
        else spawner.ReceiveEnemy(rightAdd.enemy);
    }

    public void UndoPreview(EnemySpawner.SpawnerPosition position)
    {
        var spawner = spawners.Find(x => x.position == position);

        spawner.infoUI.highlight.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);


        if (position == EnemySpawner.SpawnerPosition.Left) spawner.RemoveEnemy(leftAdd.enemy);
        else spawner.RemoveEnemy(rightAdd.enemy);
    }

    private void CloseUpgradePanel()
    {
        upgradeCards.ForEach(x => x.SetActive(false));
        upgradesOnCooldown.ForEach(x => x.SetActive(false));
        upgradePanel.SetActive(false);
        Time.timeScale = 1;
        AudioManager.Main.UnpauseAudio();

        firstCard.GetComponent<EventTrigger>().triggers[1].callback.RemoveAllListeners();
        firstCard.GetComponent<EventTrigger>().triggers[2].callback.RemoveAllListeners();
        firstCard.GetComponent<EventTrigger>().triggers[3].callback.RemoveAllListeners();
        secondCard.GetComponent<EventTrigger>().triggers[1].callback.RemoveAllListeners();
        secondCard.GetComponent<EventTrigger>().triggers[2].callback.RemoveAllListeners();
        secondCard.GetComponent<EventTrigger>().triggers[3].callback.RemoveAllListeners();

        firstCard = null;
        secondCard = null;
    }

    public void RaiseMarkerSpeed()
    {
        var marker = FindObjectOfType<ActionMarker>();
        marker.OnReset += marker.RaiseSpeed;
        CloseUpgradePanel();
    }


    public void AddRow(GameObject upgradeCard)
    {
        if (rowsAdded == 0)
        {
            thirdRows.ForEach(x => x.SetActive(true));
            rowsAdded++;
        } else
        {
            fourthRows.ForEach(x => x.SetActive(true));
            if (upgradeCards.Contains(upgradeCard)) upgradeCards.Remove(upgradeCard);
            else if (upgradesOnCooldown.Contains(upgradeCard)) upgradesOnCooldown.Remove(upgradeCard);
            Destroy(upgradeCard);
        }

        CloseUpgradePanel();
    }

    public void AddLine(GameObject upgradeCard)
    {
        switch (linesAdded)
        {
            case 0:
                secondLine.SetActive(true);
                linesAdded++;
                break;
            case 1:
                thirdLine.SetActive(true);
                linesAdded++;
                break;
            case 2:
                fourthLine.SetActive(true);
                upgradeCards.Remove(upgradeCard);
                if (upgradeCards.Contains(upgradeCard)) upgradeCards.Remove(upgradeCard);
                else if (upgradesOnCooldown.Contains(upgradeCard)) upgradesOnCooldown.Remove(upgradeCard);
                Destroy(upgradeCard);
                break;
        }

        CloseUpgradePanel();
    }

    public void RaiseMaxHealth()
    {
        FindObjectOfType<CrystalManager>().RaiseMaxHealth(10);

        CloseUpgradePanel();
    }

    public void RaiseExpMultiplier()
    {
        expMultiplier += .2f;
        multiplierValue.text = "x" + expMultiplier.ToString() + " EXP";

        CloseUpgradePanel();
    }
}
