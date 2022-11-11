using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionTile : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int weaponID;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private List<ExtraTileBackground> extraTiles;
    private List<WeaponBase> weaponsToActivate = new List<WeaponBase>();
    protected Image image;
    protected bool active = false;
    public bool handler = false;
    public bool draggable = true;
    public bool IsOverReseter { get; private set; }
    protected bool moving;
    protected InputManager pointerHandler;
    protected RectTransform rect;
    protected GameObject tileBox;
    private WeaponKey key;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        pointerHandler = InputManager.Main;
        if (handler) return;
        image = GetComponent<Image>();
        image.raycastTarget = false;
        image.maskable = false;
        DeactivateTile();
    }

    public virtual void Initialize(GameObject box)
    {
        tileBox = box;
        tileBox.tag = "FilledNode";
        tileBox.GetComponent<ActionBox>().inactive = true;
        pointerHandler.currentHoveredBox = null;
        if(draggable) image.raycastTarget = true;

        var list = FindObjectsOfType<WeaponBase>(true).ToList();
        weaponsToActivate = list.FindAll(x => x.ID == weaponID);

        var rdm = UnityEngine.Random.Range(0, 7);
        key = (WeaponKey)rdm;


        foreach (WeaponBase weapon in weaponsToActivate)
        {
            if (!weapon.gameObject.activeSelf)
            {
                weapon.unlocked = true;
                weapon.gameObject.SetActive(true);
                weapon.graphicsController.SetPreview(false);
                WeaponMasterController.Main.RegisterWeaponClass(weapon);
            }
        }

        ActionMarker.Main.OnReset += ActivateTile;
        ActivateTile();
        if (ActionMarker.Main.On == false) ActionMarker.Main.On = true;
    }

    protected virtual void ActivateTile()
    {
        image.overrideSprite = null;
        extraTiles.ForEach(x => x.Activate());
        active = true;
    }

    public void SetKey(int key)
    {
        this.key = (WeaponKey)key;
    }

    protected virtual void DeactivateTile()
    {
        image.overrideSprite = inactiveSprite;
        extraTiles.ForEach(x => x.Deactivate());
        active = false;
    }

    public virtual void Activate()
    {
        if (!active || moving) return;
        weaponsToActivate.ForEach(x => x.Shoot(key));
        DeactivateTile();
    }

    public virtual void ExitTile()
    {
        weaponsToActivate.ForEach(x => x.Stop());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Reseter" || collision.GetComponent<ActionTile>() != null)
        {
            IsOverReseter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Reseter" || collision.GetComponent<ActionTile>() != null)
        {
            IsOverReseter = false;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable) return;
        weaponsToActivate.ForEach(x => x.Stop());
        var ui = GameObject.Find("--------- UI");
        transform.SetParent(ui.transform);
        moving = true;
        image.raycastTarget = false;
        Cursor.visible = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!draggable) return;
        rect.anchoredPosition = CalculatePointerPosition();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!draggable) return;
        image.raycastTarget = true;
        moving = false;
        Cursor.visible = true;
        if (pointerHandler.currentHoveredBox != null && !pointerHandler.currentTileInstance.IsOverReseter)
        {
            rect.SetParent(pointerHandler.currentHoveredBox.transform);
            rect.anchoredPosition = Vector2.zero;
            tileBox.GetComponent<ActionBox>().inactive = false;
            tileBox = pointerHandler.currentHoveredBox;
            tileBox.GetComponent<ActionBox>().inactive = true;
        } else if(InputManager.Main.IsOverTrash)
        {
            TrashButton.Main.RecycleTile();
            DestroyTile();
        }
        else
        {
            rect.SetParent(tileBox.transform);
            rect.anchoredPosition = Vector2.zero;
        }

        weaponsToActivate.ForEach(x => x.ResetTriggers());
    }

    private Vector2 CalculatePointerPosition()
    {
        var position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        position -= Vector3.one * 0.5f;

        position.x *= 640;
        position.y *= 360;

        return position;
    }

    public void DestroyTile()
    {
        tileBox.GetComponent<ActionBox>().inactive = false;
        ActionMarker.Main.OnReset -= ActivateTile;
        Destroy(gameObject);
    }
}
