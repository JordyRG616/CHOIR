using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionTile : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Sprite sprite;
    public TileType type;
    public int cost;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private List<ExtraTileBackground> extraTiles;
    public WeaponBase weaponToActivate { get; private set; }
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

    public bool unlockWeapon = true;

    public delegate void TileActivationEvent(WeaponBase weapon);
    public TileActivationEvent OnPreActivation;
    public TileActivationEvent OnDeactivation;

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

    public void ReceiveWeapon(WeaponBase weapon)
    {
        weaponToActivate = weapon;
        weapon.OnSell += DestroyTile;
    }

    public virtual void Initialize(GameObject box)
    {
        tileBox = box;
        tileBox.tag = "FilledNode";
        var _b = tileBox.GetComponent<ActionBox>();
        _b.ReceiveTile(this);
        _b.inactive = true;
        pointerHandler.currentHoveredBox = null;
        if(draggable) image.raycastTarget = true;
        
        //var rdm = UnityEngine.Random.Range(0, 7);
        //key = (WeaponKey)rdm;

        ActionMarker.Main.OnReset += ActivateTile;
        ActivateTile();

        TutorialManager.Main.RequestTutorialPage(17);
    }

    protected virtual void ActivateTile()
    {
        OnPreActivation?.Invoke(weaponToActivate);
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
        OnDeactivation?.Invoke(weaponToActivate);
        image.overrideSprite = inactiveSprite;
        extraTiles.ForEach(x => x.Deactivate());
        active = false;
    }

    public virtual void Activate()
    {
        if (!active || moving) return;
        weaponToActivate.Shoot(key);
        DeactivateTile();
    }

    public virtual void ExitTile()
    {
        weaponToActivate.Stop();
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
            var _b = tileBox.GetComponent<ActionBox>();
            _b.inactive = false;
            _b.RemoveTile();
            tileBox = pointerHandler.currentHoveredBox;
            _b = tileBox.GetComponent<ActionBox>();
            _b.ReceiveTile(this);
            _b.inactive = true;
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

        position.x *= 1280;
        position.y *= 720;

        return position;
    }

    public void DestroyTile()
    {
        tileBox.GetComponent<ActionBox>().inactive = false;
        ActionMarker.Main.OnReset -= ActivateTile;
        Destroy(gameObject);
    }
}

public enum TileType { Single, Double, TwoBar, ThreeBar}