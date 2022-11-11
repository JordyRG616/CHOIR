using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanelSlider : MonoBehaviour
{
    [SerializeField] private Vector2 positionLimits;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Slider slider;
    private float _pos;
    private float panelPosition
    {
        get => _pos;
        set
        {
            if (value <= 0) _pos = 0;
            else if (value >= 1) _pos = 1;
            else _pos = value;
        }
    }

    private void OnEnable()
    {
        panelPosition = 0;
    }

    private void Update()
    {
        if(Input.mouseScrollDelta.magnitude != 0)
        {
            panelPosition -= Input.mouseScrollDelta.y / 5;
            slider.value = panelPosition;
        }

        var pos = Mathf.Lerp(positionLimits.x, positionLimits.y, panelPosition);
        var _p = new Vector2(rect.anchoredPosition.x, pos);
        rect.anchoredPosition = _p;
    }

    public void SetPanelPosition(float value)
    {
        panelPosition = value;
    }
}
