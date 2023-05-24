using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameLog : MonoBehaviour
{
    #region Main
    private static EndGameLog _instance;
    public static EndGameLog Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<EndGameLog>();

            return _instance;
        }

    }
    #endregion

    private float seconds = 0;
    private int minutes;
    private int waves = 0;
    private int health = 0;

    [Header("Score")]
    [SerializeField] private float pointsWeight;
    public int enemies = 0;
    [SerializeField] private float enemyWeight;
    public int damageTaken;
    [SerializeField] private float damageWeight;
    public int weaponLevel;
    [SerializeField] private float levelWeight;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI finalScore;
    [Space]
    [SerializeField] private TextMeshProUGUI pointsValue;
    [SerializeField] private TextMeshProUGUI pointsMultiplier;
    [SerializeField] private TextMeshProUGUI pointsTotal;
    [Space]
    [SerializeField] private TextMeshProUGUI enemiesValue;
    [SerializeField] private TextMeshProUGUI enemiesMultiplier;
    [SerializeField] private TextMeshProUGUI enemiesTotal;
    [Space]
    [SerializeField] private TextMeshProUGUI damageValue;
    [SerializeField] private TextMeshProUGUI damageMultiplier;
    [SerializeField] private TextMeshProUGUI damageTotal;
    [Space]
    [SerializeField] private TextMeshProUGUI levelValue;
    [SerializeField] private TextMeshProUGUI levelMultiplier;
    [SerializeField] private TextMeshProUGUI levelTotal;
    private WaitForSecondsRealtime longerWait = new WaitForSecondsRealtime(0.25f);


    private void Update()
    {
        seconds += Time.deltaTime;
        if(seconds >= 60)
        {
            minutes++;
            seconds -= 60f;
        }
    }

    public void ChangeWeight(WeightType weight, float value)
    {
        switch(weight)
        {
            case WeightType.EnemiesKilled:
                enemyWeight += value;
            break;
            case WeightType.DamageTaken:
                damageWeight += value;
            break;
            case WeightType.SpareMoney:
                pointsWeight += value;
            break;
            case WeightType.WeaponLevel:
                levelWeight += value;
            break;
        }
    }

    public void TriggerEndgame(bool victory)
    {
        Debug.Log("Triggered");
        panel.SetActive(true);

        if (victory) 
        {
            title.text = "victory";
            title.color = Color.green;
        }
        else 
        {
            title.text = "defeat";
            title.color = Color.red;
        }

        pointsMultiplier.text = "x" + pointsWeight;
        enemiesMultiplier.text = "x" + enemyWeight;
        damageMultiplier.text = "x" + damageWeight;
        levelMultiplier.text = "x" + levelWeight;

        StartCoroutine(CalculateFinalScore());

        Time.timeScale = 0;
    }

    private IEnumerator CalculateFinalScore()
    {
        yield return longerWait;
        int container = 0;

        var pValue = Mathf.RoundToInt(CrystalManager.Main.buildPoints * pointsWeight);
        var waitTime = 0.05f;

        for (int i = 0; i < pValue; i++)
        {
            container++;
            pointsValue.text = container.ToString();
            yield return new WaitForSecondsRealtime(waitTime);
            if(waitTime > 0.01f) waitTime -= 0.005f;
        }
        pointsTotal.text = "=" + pValue.ToString();

        yield return longerWait;

        var lValue = Mathf.RoundToInt(weaponLevel * levelWeight);
        container = 0;
        waitTime = 0.05f;
        for (int i = 0; i < lValue; i++)
        {
            container++;
            levelValue.text = container.ToString();
            yield return new WaitForSecondsRealtime(waitTime);
            if(waitTime > 0.01f) waitTime -= 0.005f;
        }
        levelTotal.text = "=" + lValue.ToString();

        yield return longerWait;

        var eValue = Mathf.RoundToInt(enemies * enemyWeight);
        container = 0;
        waitTime = 0.05f;
        for (int i = 0; i < eValue; i++)
        {
            container++;
            enemiesValue.text = container.ToString();
            yield return new WaitForSecondsRealtime(waitTime);
            if(waitTime > 0.01f) waitTime -= 0.005f;
        }
        enemiesTotal.text = "=" + eValue.ToString();

        yield return longerWait;

        var dValue = -Mathf.RoundToInt(damageTaken * damageWeight);
        container = 0;
        waitTime = 0.05f;
        for (int i = 0; i < dValue; i++)
        {
            container++;
            damageValue.text = container.ToString();
            yield return new WaitForSecondsRealtime(waitTime);
            if(waitTime > 0.01f) waitTime -= 0.005f;
        }
        damageTotal.text = "=" + dValue.ToString();

        yield return longerWait;

        

        var score = lValue + pValue + dValue + eValue;
        
        Inventory.Main.ReceiveEndlevelValues(score, CrystalManager.Main.currentHealth);

        score = Mathf.Max(0, score);

        finalScore.text = score.ToString();

    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        var i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i);
        AudioManager.Main.UnpauseAudio();
    }

    public void ToNextLevel()
    {
        Time.timeScale = 1;

        Inventory.Main.ResetList();

        var scene = AlbumManager.Main.GetNextLevelName();
        SceneManager.LoadScene(scene);
    }
}

public enum WeightType {EnemiesKilled, WeaponLevel, DamageTaken, SpareMoney}