using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DoubleTileHandler : ActionTile
{
    [SerializeField] private List<ActionTile> tiles;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        pointerHandler = InputManager.Main;
    }

    public override void Initialize(GameObject box)
    {
        tileBox = box;
        tiles.ForEach(x => x.Initialize(box));
        image.raycastTarget = true;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        var ui = GameObject.Find("--------- UI");
        image.raycastTarget = false;
        transform.SetParent(ui.transform);
        Cursor.visible = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition = CalculatePointerPosition();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        Cursor.visible = true;
        if (pointerHandler.currentHoveredBox != null && !pointerHandler.currentTileInstance.IsOverReseter)
        {
            rect.SetParent(pointerHandler.currentHoveredBox.transform);
            rect.anchoredPosition = Vector2.zero;
            tileBox.GetComponent<ActionBox>().inactive = false;
            tileBox = pointerHandler.currentHoveredBox;
            tileBox.GetComponent<ActionBox>().inactive = true;
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
}
