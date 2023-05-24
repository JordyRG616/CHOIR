using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Main
    private static InputManager _instance;
    public static InputManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<InputManager>();

            return _instance;
        }

    }
    #endregion

    public ActionTile currentTileInstance;
    public GameObject currentHoveredBox;
    public bool IsOverTrash;

    public bool waitingForSlot = false;
    public bool draggingWeapon = false;
    public Transform weaponDragged = null;
    public WeaponSlot hoveredSlot = null;
    public bool selectingUpgrade = false;
    public WeaponSlot selectedSlot = null;
    public WeaponBase weaponToPlace;
    public ModeButton selectedButton;
    public InteractionMode _mode;
    public InteractionMode interactionMode
    {
        get => _mode;
        set
        {
            _mode = value;
            if(_mode == InteractionMode.Default)
            {
                specialCursor.Deactivate();
                Cursor.visible = true;
            } else
            {
                specialCursor.SetActive(_mode);
                Cursor.visible = false;
            }
        }
    }

    [Header("UI")]
    [SerializeField] private KeyCode showUIKey;
    [SerializeField] private CanvasGroup uiGroup;
    [SerializeField] private GameObject extraBG;
    [SerializeField] private SpecialCursor specialCursor;

    private bool _open;
    public bool uiOpen
    {
        get => _open;
        set
        {
            _open = value;
            if(_open)
            {
                uiGroup.alpha = 1;
                uiGroup.interactable = true;
                uiGroup.blocksRaycasts = true;
                extraBG.SetActive(true);
            } else
            {
                uiGroup.alpha = 0;
                uiGroup.blocksRaycasts = false;
                uiGroup.interactable = false;
                extraBG.SetActive(false);
            }
        }
    }

    private bool paused = false;

    public void SetHoveredBox(GameObject box)
    {
        currentHoveredBox = box;
    }

    public void SetActionTile()
    {
        var rect = currentTileInstance.GetComponent<RectTransform>();
        rect.SetParent(currentHoveredBox.transform);
        rect.anchoredPosition = Vector2.zero;
        currentTileInstance.Initialize(currentHoveredBox);
    }

    private void Update()
    {
        if(Input.GetKeyDown(showUIKey))
        {
            uiOpen = !uiOpen;
        }
    }

    public void SetOverTrash(bool value)
    {
        IsOverTrash = value;
    }
}

public enum InteractionMode  { Default, Build, Upgrade, Move, Sell }