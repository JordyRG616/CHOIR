using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponList : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    #region Main
    private static WeaponList _instance;
    public static WeaponList Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<WeaponList>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private WeaponSelector selectorModel;
    [SerializeField] private RectTransform panel;
    [SerializeField] private Vector2 boundaries;
    private RectTransform self;
    private List<GameObject> boxes = new List<GameObject>();
    [SerializeField] private float scrollSpeed;
    private bool pointerIsInside;
    private float _pos;
    private float panelPosition
    {
        get => _pos;
        set
        {
            if(value < 0) _pos = 0;
            else if(value > maxPos) _pos = maxPos;
            else _pos = value;
        }
    }
    private float maxPos
    {
        get
        {
            if(panel.childCount <= 3) return 0;
            else 
            {
                var count = panel.childCount - 3;
                return ((count * 40) + 10f) + (count * 6);
            }
        }
    }


    void Start()
    {
        self = GetComponent<RectTransform>();
    }

    public void Open(List<WeaponBase> list)
    {
        foreach(var weapon in list)
        {
            var box = Instantiate(selectorModel, panel);
            box.ReceiveWeapon(weapon);
            boxes.Add(box.gameObject);
        }

        SetPosition();
    }

    public void Close()
    {
        boxes.ForEach(x => Destroy(x));
        boxes.Clear();
        self.anchoredPosition = Vector2.one * 1000;
    }

    private void SetPosition()
    {
        var pos = GlobalFunctions.CalculatePointerPosition();
        var pivot = new Vector2(1.05f, -0.05f);

        if(pos.x < boundaries.x) pivot.x = -0.05f;
        if(pos.y > boundaries.y) pivot.y = 1.05f;

        self.anchoredPosition = pos;
        self.pivot = pivot;
    }

    void Update()
    {
        if(pointerIsInside)
        {
            panelPosition -= Input.mouseScrollDelta.y * scrollSpeed;
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, panelPosition);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIsInside = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        panelPosition += eventData.delta.y;
        panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, panelPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerIsInside = false;
    }
}
