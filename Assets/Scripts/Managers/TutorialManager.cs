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

    [SerializeField] private GameObject panel;
    [SerializeField] private List<GameObject> pages;
    private List<int> usedIndexes = new List<int>();
    private bool skipTutorial;


    private void Start()
    {
        pages[0].SetActive(true);
        usedIndexes.Add(0);
    }

    public void RequestTutorialPage(int pageIndex, int steps = 1, bool cursorPosition = false)
    {
        if (skipTutorial || usedIndexes.Contains(pageIndex)) return;

        panel.SetActive(true);
        StartCoroutine(OpenPage(pageIndex, steps, cursorPosition));
        
    }

    private IEnumerator OpenPage(int index, int steps, bool cursorPosition = false)
    {
        Vector2 pos = GlobalFunctions.CalculatePointerPosition();

        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForSeconds(0.05f);

            var page = pages[index + i];
            page.SetActive(true);

            if ((cursorPosition))
            {
                page.GetComponent<RectTransform>().anchoredPosition = pos;
            }

            yield return new WaitUntil(() => Input.anyKeyDown == true);

            usedIndexes.Add(index + i);
            page.SetActive(false);
        }

        panel.SetActive(false);
    }

    public void SkipTutorial(bool skip)
    {
        skipTutorial = skip;

        if (!skip) RequestTutorialPage(1, 3);
    }
}
