using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SupportCard : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SupportTile supportTile;
    private int cachedIndex;
    private RectTransform tileRect;
    private LayoutElement layout;
    private InputManager inputManager;

    private void Start()
    {
        inputManager = InputManager.Main;
        layout = gameObject.AddComponent<LayoutElement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        inputManager.currentSupportCard = this;
        inputManager.currentSupportTile = Instantiate(supportTile, GlobalFunctions.CalculatePointerPosition(), Quaternion.identity, GameObject.FindGameObjectWithTag("MainUI").transform);
        tileRect = inputManager.currentSupportTile.GetComponent<RectTransform>();
        Cursor.visible = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        tileRect.anchoredPosition = GlobalFunctions.CalculatePointerPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Cursor.visible = true;
        if (inputManager.currentHoveredBox != null && !inputManager.currentSupportTile.IsOverReseter)
        {
            inputManager.SetSupportTile();
        }
        else
        {
            Destroy(inputManager.currentSupportTile.gameObject);
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
