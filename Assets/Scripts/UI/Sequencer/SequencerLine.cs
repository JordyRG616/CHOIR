using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerLine : MonoBehaviour
{
    public WeaponKey key;
    [SerializeField] private List<ActionBox> nodesInLine;


    private void Start()
    {
        nodesInLine.ForEach(x => x.line = this);
    }

    public void FillExtraNodes(ActionBox box, int size, WeaponBase weapon)
    {
        var index = nodesInLine.IndexOf(box);
        for (int i = 1; i < size; i++)
        {
            nodesInLine[index - i].SetWeaponInColumn(weapon);
        }
    }

    public void EmptyExtraNodes(ActionBox box, int size, WeaponBase weapon)
    {
        var index = nodesInLine.IndexOf(box);
        for (int i = 1; i < size; i++)
        {
            nodesInLine[index - i].RemoveWeaponInColumn(weapon);
        }
    }
}
