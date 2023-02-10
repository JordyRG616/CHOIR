using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileCreator : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public TileType tileType;
    [SerializeField] private TMPro.TextMeshProUGUI costText;
    private ActionTile tile;
    private int cost;
    private CrystalManager crystal;
    private InputManager pointerHandler;
    private RectTransform actionTileRect;

    private void Start()
    {
        pointerHandler = InputManager.Main;
        crystal = CrystalManager.Main;
    }

    public void ReceiveTile(ActionTile tile)
    {
        this.tile = tile;
        cost = tile.cost;

        costText.text = cost.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (crystal.buildPoints < cost) return;

        pointerHandler.currentTileInstance = Instantiate(tile, GlobalFunctions.CalculatePointerPosition(), Quaternion.identity, GameObject.FindGameObjectWithTag("MainUI").transform);
        pointerHandler.currentTileInstance.ReceiveWeapon(tile.weaponToActivate);
        pointerHandler.currentTileInstance.unlockWeapon = false;
        actionTileRect = pointerHandler.currentTileInstance.GetComponent<RectTransform>();
        //Cursor.visible = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        actionTileRect.anchoredPosition = GlobalFunctions.CalculatePointerPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Cursor.visible = true;
        if (pointerHandler.currentHoveredBox != null && !pointerHandler.currentTileInstance.IsOverReseter)
        {
            pointerHandler.SetActionTile(false);
            crystal.ExpendBuildPoints(cost);
        }
        else
        {
            Destroy(pointerHandler.currentTileInstance.gameObject);
        }
    }

    public void SetTileWeapon(WeaponBase weapon)
    {
        if (tile != null) tile.ReceiveWeapon(weapon);
    }
}
