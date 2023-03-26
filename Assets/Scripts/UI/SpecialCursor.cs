using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class SpecialCursor : MonoBehaviour
{
    #region Main
    private static SpecialCursor _instance;
    public static SpecialCursor Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SpecialCursor>();

            return _instance;
        }

    }
    #endregion


    [System.Serializable]
    public struct CursorGraphic
    {
        public InteractionMode mode;
        public Sprite icon;
    }

    [SerializeField] private List<CursorGraphic> graphics;
    [SerializeField] private Light2D cursorLight;
    [SerializeField] private GameObject costBox;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI upgradeDescription;

    private Image cursor;
    private RectTransform rect;


    void Awake()
    {
        cursor = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void SetActive(InteractionMode mode)
    {
        var icon = graphics.Find(x => x.mode == mode).icon;
        cursor.sprite = icon;
        cursor.enabled = true;
        cursorLight.gameObject.SetActive(true);
        SetAvailable(false);
    }

    public void Deactivate()
    {
        cursor.enabled = false;
        cursorLight.gameObject.SetActive(false);
    }

    public void SetAvailable(bool value, int cost = 0, string desc = "")
    {
        if(!value) 
        {
            cursor.color = Color.red;
            cursorLight.color = Color.red;
        } else
        {
            cursor.color = Color.green;
            cursorLight.color = Color.green;
        }

        if(cost > 0)
        {
            upgradeCost.text = cost + " $";
            costBox.SetActive(true);
        } else
        {
            costBox.SetActive(false);
        }

        if(desc != "")
        {
            upgradeDescription.gameObject.SetActive(true);
            upgradeDescription.text = desc;
        } else
        {
            upgradeDescription.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        rect.anchoredPosition = GlobalFunctions.CalculatePointerPosition();
    }
}