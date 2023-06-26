using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ActionTile storedTile;
    public bool occupied => storedTile != null;

    [HideInInspector] public SequencerLine line;
    [SerializeField] private SequencerColumn column;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (occupied) return;
        InputManager.Main.currentHoveredBox = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (occupied) return;
        InputManager.Main.currentHoveredBox = null;
    }

    public void ReceiveTile(ActionTile tile)
    {
        storedTile = tile;
        tile.transform.localPosition = new Vector3(0, .5f, 0);
        AudioManager.Main.RequestEvent(FixedAudioEvent.PlaceTile);
        column.weaponsInColumn.Add(tile.weaponToActivate);
        if(tile.size > 1) line.FillExtraNodes(this, tile.size, tile.weaponToActivate);
    }

    public void RemoveTile()
    {
        column.weaponsInColumn.Remove(storedTile.weaponToActivate);
        if(storedTile.size > 1) line.EmptyExtraNodes(this, storedTile.size, storedTile.weaponToActivate);
        storedTile = null;
    }

    public bool CanReceiveTile(WeaponBase weapon)
    {
        return !column.weaponsInColumn.Contains(weapon);
    }

    public void SetWeaponInColumn(WeaponBase weapon)
    {
        column.weaponsInColumn.Add(weapon);
    }

    public void RemoveWeaponInColumn(WeaponBase weapon)
    {
        column.weaponsInColumn.Remove(weapon);
    }
}
