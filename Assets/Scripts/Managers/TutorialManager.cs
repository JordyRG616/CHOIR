using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region Main
    private static TutorialManager _instance;
    public static TutorialManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<TutorialManager>();

            return _instance;
        }

    }
    #endregion

    public RectTransform tutorialBox;
    public TMPro.TextMeshProUGUI tutorialText;
    public GameObject panel;
    [Space]
    public List<TutorialStep> tutorialSteps;
    public bool tutorialOn;
    public bool paused;

    public void DoTutorialStep(string key, bool pause = false)
    {
        if (tutorialSteps.Count == 0) return;

        if (PlayerPrefs.GetInt("TutorialTaken") == -1)
        {
            tutorialSteps.Clear();
            return;
        }

        var step = tutorialSteps.Find(x => x.tutorialKey == key);
        if (step == null) return;
        if (pause)
        {
            Time.timeScale = 0;
            paused = true;
        }
        tutorialBox.anchoredPosition = step.boxPosition;
        tutorialText.text = step.text;

        panel.SetActive(true);
        tutorialBox.gameObject.SetActive(true);
        tutorialOn = true;

        tutorialSteps.Remove(step);
    }

    public void CloseTutorialPanel()
    {
        if(paused)
        {
            Time.timeScale = 1;
            paused = false;
        }

        panel.SetActive(false);
        tutorialBox.gameObject.SetActive(false);
        tutorialOn = false;

        if(tutorialSteps.Count == 0)
        {
            PlayerPrefs.SetInt("TutorialTaken", -1);
        }
    }
}

[System.Serializable]
public class TutorialStep
{
    public string tutorialKey;
    public Vector2 boxPosition;
    [TextArea] public string text;
}