using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionTile : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite sprite;
    public int cost;
    public WeaponBase weaponToActivate { get; private set; }
    private WeaponGraphicsController graphicsController;
    protected Image image;
    protected bool active = false;
    public bool draggable = true;
    public bool IsOverReseter { get; private set; }
    protected bool moving;
    protected InputManager pointerHandler;
    protected RectTransform rect;
    protected GameObject tileBox;
    private WeaponKey key;
    private ActionBox box;

    public bool unlockWeapon = true;

    public delegate void TileActivationEvent(WeaponBase weapon);
    public TileActivationEvent OnPreActivation;
    public TileActivationEvent OnDeactivation;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        pointerHandler = InputManager.Main;
        image = GetComponent<Image>();
        image.raycastTarget = false;
        image.maskable = false;
        DeactivateTile();
    }

    public void ReceiveWeapon(WeaponBase weapon)
    {
        weaponToActivate = weapon;
        graphicsController = weaponToActivate.GetComponent<WeaponGraphicsController>();
    }

    public virtual void Initialize(GameObject box)
    {
        tileBox = box;
        tileBox.tag = "FilledNode";
        this.box = tileBox.GetComponent<ActionBox>();
        this.box.ReceiveTile(this);
        pointerHandler.currentHoveredBox = null;
        if(draggable) image.raycastTarget = true;

        weaponToActivate.ReceiveTile(this);

        ActionMarker.Main.OnReset += ActivateTile;
        ActivateTile();
    }
    
    protected virtual void ActivateTile()
    {
        OnPreActivation?.Invoke(weaponToActivate);
        image.color = Color.white;
        active = true;
    }

    public void SetKey(int key)
    {
        this.key = (WeaponKey)key;
    }

    protected virtual void DeactivateTile()
    {
        OnDeactivation?.Invoke(weaponToActivate);
        active = false;
    }

    public virtual void Activate()
    {
        if (!active || moving) return;
        weaponToActivate.Shoot(key);
        DeactivateTile();
        image.color = Color.gray;
    }

    public virtual void ExitTile()
    {
        weaponToActivate.Stop();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Reseter" || collision.tag == "Blocker" || collision.GetComponent<ActionTile>() != null)
        {
            IsOverReseter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Reseter" || collision.tag == "Blocker" || collision.GetComponent<ActionTile>() != null)
        {
            IsOverReseter = false;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable) return;
        weaponToActivate.Stop();
        var ui = GameObject.FindGameObjectWithTag("MainUI");
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
            box.RemoveTile();
            tileBox = pointerHandler.currentHoveredBox;
            box = tileBox.GetComponent<ActionBox>();
            box.ReceiveTile(this);
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
        ActionMarker.Main.OnReset -= ActivateTile;
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        graphicsController.SetHighlighted(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        graphicsController.SetHighlighted(false);
    }

}
