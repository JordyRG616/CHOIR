using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool inactive = false;
    private ActionTile storedTile;
    public bool occupied => storedTile != null;

    public delegate void TilePlacementeEvent(ActionTile tile);
    public TilePlacementeEvent OnTilePlaced;
    public TilePlacementeEvent OnTileRemoval;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inactive) return;
        InputManager.Main.currentHoveredBox = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inactive) return;
        InputManager.Main.currentHoveredBox = null;
    }

    public void ReceiveTile(ActionTile tile)
    {
        storedTile = tile;
        OnTilePlaced?.Invoke(tile);
    }

    public void RemoveTile()
    {
        OnTileRemoval?.Invoke(storedTile);
        storedTile = null;
    }
}
