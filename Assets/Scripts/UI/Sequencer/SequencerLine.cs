using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerLine : MonoBehaviour
{
    public WeaponKey key;
    [SerializeField] private List<ActionBox> nodesInLine;

    public ActionBox.TilePlacementeEvent OnNewTilePlaced;
    public ActionBox.TilePlacementeEvent OnTileRemoved;
    

    private void Start()
    {
        nodesInLine.ForEach(x =>
        {
            x.OnTilePlaced += SetTileInfo;
            x.OnTileRemoval += RemoveTile;
        });
    }

    private void SetTileInfo(ActionTile tile)
    {
        tile.SetKey((int)key);
        OnNewTilePlaced?.Invoke(tile);
    }

    private void RemoveTile(ActionTile tile)
    {
        OnTileRemoved?.Invoke(tile);
    }
}
