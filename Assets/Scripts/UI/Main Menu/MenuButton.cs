using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image background, frame;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private int ogTextSize, hoverSize;
    [SerializeField] private Color ogFrameColor, ogBackColor, pressedColor;
    [ColorUsage(true, true)] [SerializeField] private Color hoverColor;

    public UnityEvent OnClick;


    public void OnPointerEnter(PointerEventData eventData)
    {
        frame.color = hoverColor;
        label.fontSize = hoverSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        frame.color = ogFrameColor;
        background.color = ogBackColor;
        label.fontSize = ogTextSize;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        background.color = ogBackColor;
        OnClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.color = pressedColor;
    }
}
