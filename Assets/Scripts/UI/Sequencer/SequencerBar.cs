using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SequencerBar : MonoBehaviour
{
    [SerializeField] private List<SequencerLine> linesInBar;


    private void Start()
    {
        GenerateChord();
    }

    private void GenerateChord()
    {
        var rdm = Random.Range(0, 7);
        linesInBar[0].key = (WeaponKey)rdm;

        rdm += 2;
        if (rdm >= 7) rdm = rdm - 7;
        linesInBar[1].key = (WeaponKey)rdm;

        rdm += 2;
        if (rdm >= 7) rdm = rdm - 7;
        linesInBar[2].key = (WeaponKey)rdm;
    }
}