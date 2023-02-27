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
    public int enemies = 0;
    private int health = 0;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI values;
    [SerializeField] private TextMeshProUGUI finalScore;

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
        panel.SetActive(true);

        if (victory) title.text = "victory";
        else title.text = "defeat";

        values.text = minutes + "m" + seconds.ToString("00") + "s\n" + CrystalManager.Main.level + "\n" + enemies + "\n" + CrystalManager.Main.currentHealth;
        finalScore.text = CalculateFinalScore().ToString();
        Time.timeScale = 0;
    }

    private int CalculateFinalScore()
    {
        int score = 0;

        for (int i = 1; i <= CrystalManager.Main.level; i++)
        {
            score += i;
        }

        score += enemies;
        score += CrystalManager.Main.currentHealth * 3;
        return score;
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
        var i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i + 1);
    }
}
