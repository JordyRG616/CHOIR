using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool inactive = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inactive) return;
        InputManager.Main.currentHoveredBox = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inactive) return;
        InputManager.Main.currentHoveredBox = null;
    }
}
