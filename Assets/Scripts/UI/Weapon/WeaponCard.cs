using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class WeaponCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ActionTile actionTile;
    [SerializeField] private CardEffect effect;
    public bool unique;
    [SerializeField] private List<WeaponCard> unlockedCards;
    public List<WeaponCard> incompatibleCards;
    private RectTransform actionTileRect;
    private InputManager pointerHandler;
    public Sprite frame;
    public Sprite Sprite;
    public string Description;
    public EnemySpawner.SpawnerPosition spawnerToUpgrade;
    public int spawnerUpgradeLevel;
    private LayoutElement layout;
    private int cachedIndex;
    public CardRarity rarity;

    private void Start()
    {
        pointerHandler = InputManager.Main;
        layout = gameObject.AddComponent<LayoutElement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        pointerHandler.currentWeaponCard = this;
        pointerHandler.currentTileInstance = Instantiate(actionTile, GlobalFunctions.CalculatePointerPosition(), Quaternion.identity, GameObject.FindGameObjectWithTag("MainUI").transform);
        actionTileRect = pointerHandler.currentTileInstance.GetComponent<RectTransform>();
        Cursor.visible = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        actionTileRect.anchoredPosition = GlobalFunctions.CalculatePointerPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Cursor.visible = true;
        if(pointerHandler.currentHoveredBox != null && !pointerHandler.currentTileInstance.IsOverReseter)
        {
            pointerHandler.SetActionTile();
            if (effect != null) effect.Apply();
            //if (unlockedCards.Count > 0) RewardManager.Main.ReceiveCard(unlockedCards);
        } else
        {
            Destroy(pointerHandler.currentTileInstance.gameObject);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.2f;
        cachedIndex = transform.GetSiblingIndex();
        layout.ignoreLayout = true;
        transform.SetAsLastSibling();
        var rect = (RectTransform)transform;
        rect.anchoredPosition += new Vector2(0, 30);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        layout.ignoreLayout = false;
        transform.SetSiblingIndex(cachedIndex);
        var rect = (RectTransform)transform;
        rect.anchoredPosition += new Vector2(0, -30);
    }
}

public enum CardRarity 
{ 
    Common = 0, 
    Uncommon = 1, 
    Rare = 2,
    Base = 3
}