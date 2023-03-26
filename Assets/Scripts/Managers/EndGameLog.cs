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

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI finalScore;
    [Space]
    [SerializeField] private TextMeshProUGUI pointsValue;
    [SerializeField] private TextMeshProUGUI pointsMultiplier;
    [Space]
    [SerializeField] private TextMeshProUGUI enemiesValue;
    [SerializeField] private TextMeshProUGUI enemiesMultiplier;
    [Space]
    [SerializeField] private TextMeshProUGUI damageValue;
    [SerializeField] private TextMeshProUGUI damageMultiplier;

    private void Update()
    {
        seconds += Time.deltaTime;
        if(seconds >= 60)
        {
            minutes++;
            seconds -= 60f;
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

        finalScore.text = CalculateFinalScore().ToString();
        Time.timeScale = 0;
    }

    private int CalculateFinalScore()
    {
        int score = 0;

        var pValue = Mathf.RoundToInt(CrystalManager.Main.buildPoints * pointsWeight);
        score += pValue;
        pointsValue.text = pValue.ToString();

        var eValue = Mathf.RoundToInt(enemies * enemyWeight);
        score += eValue;
        enemiesValue.text = eValue.ToString();

        var dValue = -Mathf.RoundToInt(damageTaken * damageWeight);
        score += dValue;
        damageValue.text = dValue.ToString();
        
        Inventory.Main.ReceiveEndlevelValues(score, CrystalManager.Main.currentHealth);

        return Mathf.Max(0, score);
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
    }

    public void ToNextLevel()
    {
        Time.timeScale = 1;
        var scene = AlbumManager.Main.GetNextLevelName();
        SceneManager.LoadScene(scene);
    }
}
