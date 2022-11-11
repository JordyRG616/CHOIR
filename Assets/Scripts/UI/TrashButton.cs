using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashButton : MonoBehaviour
{
    #region Main
    private static TrashButton _instance;
    public static TrashButton Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<TrashButton>();

            return _instance;
        }
    }
    #endregion

    [SerializeField] private RectTransform bar;
    [SerializeField] private float maxBarSize;
    [SerializeField] private int recycleCost;
    private int actualCount;

    private void Start()
    {
        bar.sizeDelta = new Vector2(0, bar.sizeDelta.y);
    }

    public void RecycleTile()
    {
        actualCount++;
        var percentage = actualCount / (float) recycleCost;
        var size = percentage * maxBarSize;

        if(actualCount == recycleCost)
        {
            actualCount = 0;
            Time.timeScale = 0;
            recycleCost++;

            RewardManager.Main.OpenReward(RewardManager.Main.defaultCards);
        }

        bar.sizeDelta = new Vector2(size, bar.sizeDelta.y);
    }
}
