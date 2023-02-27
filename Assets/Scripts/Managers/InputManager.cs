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
    public InteractionMode interactionMode;

    [Header("UI")]
    [SerializeField] private KeyCode showUIKey;
    [SerializeField] private List<Animator> animators;

    private bool _open;
    public bool uiOpen
    {
        get => _open;
        set
        {
            _open = value;
            animators.ForEach(x => x.SetBool("Open", _open));
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;

            if (paused) Time.timeScale = 1;
            else Time.timeScale = 0;
        }

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