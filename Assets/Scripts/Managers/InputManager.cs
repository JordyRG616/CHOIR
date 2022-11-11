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

    public WeaponCard currentWeaponCard;
    public ActionTile currentTileInstance;
    public GameObject currentHoveredBox;
    public bool IsOverTrash;

    public void SetHoveredBox(GameObject box)
    {
        currentHoveredBox = box;
    }

    public void SetTile()
    {
        var rect = currentTileInstance.GetComponent<RectTransform>();
        rect.SetParent(currentHoveredBox.transform);
        rect.anchoredPosition = Vector2.zero;
        currentTileInstance.Initialize(currentHoveredBox);
        Destroy(currentWeaponCard.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3;
        }
    }

    public void SetOverTrash(bool value)
    {
        IsOverTrash = value;
    }
}

