using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerBlocker : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private TMPro.TextMeshProUGUI costTxt;

    private void Start()
    {
        costTxt.text = cost.ToString();
    }

    public void Unlock()
    {
        if (CrystalManager.Main.buildPoints >= cost)
        {
            gameObject.SetActive(false);
        } else
        {
            CrystalManager.Main.BlinkCost();
        }

    }
}
