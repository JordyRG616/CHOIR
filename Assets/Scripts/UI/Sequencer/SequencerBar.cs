using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SequencerBar : MonoBehaviour
{
    [SerializeField] private List<SequencerLine> linesInBar;
    [SerializeField] private List<ChordPart> chordParts;
    private ChordPart selectedChord;


    private void Start()
    {
        GenerateChord();

        linesInBar.ForEach(x =>
        {
            x.OnNewTilePlaced += RegisterNewTile;
            x.OnTileRemoved += RemoveTile;
        });

        var rdm = Random.Range(0, chordParts.Count);
        selectedChord = chordParts[rdm];
        selectedChord.Setup();
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

    private void RegisterNewTile(ActionTile tile)
    {
        if(HasClass(tile))
        {
            selectedChord.ReceiveTile();
        }
    }

    private void RemoveTile(ActionTile tile)
    {
        if (HasClass(tile))
        {
            selectedChord.RemoveTile();
        }
    }

    private bool HasClass(ActionTile tile)
    {
        var _class = tile.weaponToActivate.classes;

        if (selectedChord.requiredClass == _class) return true;
        return false;
    }
}

[System.Serializable]
public class ChordPart
{
    public WeaponClass requiredClass;
    public Sprite classIcon;
    [SerializeField] private ChordBonus bonus;
    [SerializeField] private int requiredTiles;
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI requirement;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private CanvasGroup canvasGroup;
    private bool complete;
    private int matchingTiles;


    public void Setup()
    {
        icon.sprite = classIcon;
        requirement.text = requiredTiles + "+";
        description.text = bonus.Description;
    }


    public void ReceiveTile()
    {
        matchingTiles++;

        if (matchingTiles == requiredTiles && !complete)
        {
            complete = true;
            canvasGroup.alpha = 1;
            bonus.Apply();
        }
    }

    public void RemoveTile()
    {
        matchingTiles--;

        if (matchingTiles < requiredTiles && complete)
        {
            complete = false;
            canvasGroup.alpha = 0;
            bonus.Remove();
        }
    }
}