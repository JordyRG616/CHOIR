using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [field: SerializeField] public Transform holder { get; private set; }
    [SerializeField] private ParticleSystem selectableVFX;
    [SerializeField] private List<int> rotations;
    [SerializeField] private float dragThreshold = 0.25f;
    [SerializeField] private GameObject dragIcon;
    private RectTransform tile;
    private float counter;
    private bool dragging;
    private float stationaryAngle;
    public Transform weapon { get; private set; }
    public WeaponBase weaponBase { get; private set; }
    private InputManager inputManager;
    public bool IsOccupied => weapon != null;
    private int _rotIndex = 0;
    private int RotationIndex
    {
        get => _rotIndex;
        set
        {
            if (value >= rotations.Count) _rotIndex = 0;
            else _rotIndex = value;
        }
    }


    private void Start()
    {
        inputManager = InputManager.Main;
        stationaryAngle = holder.eulerAngles.z;
        StartCoroutine(ManageSelectableVFX());
    }

    private IEnumerator ManageSelectableVFX()
    {
        var waitToActivate = new WaitUntil(() => inputManager.waitingForSlot == true);
        var waitToDeactivate = new WaitUntil(() => inputManager.waitingForSlot == false);

        while (true)
        {
            yield return waitToActivate;

            selectableVFX.Play();

            yield return waitToDeactivate;

            selectableVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void PlaySelectableEffect()
    {
        selectableVFX.Play();
    }

    public void StopSelectableEffect()
    {
        selectableVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void UnlockDirectionalButtons(string[] keys)
    {
        
    }

    private void OnMouseUp()
    {
        if(dragging)
        {
            Cursor.visible = true;
            if (inputManager.currentHoveredBox != null && !inputManager.currentTileInstance.IsOverReseter)
            {
                inputManager.SetActionTile(false);
                CrystalManager.Main.ExpendBuildPoints(weaponBase.tiles[0].cost);
            }
            else
            {
                Destroy(tile.gameObject);
            }
            dragging = false;
        } else if(counter < dragThreshold)
        {
            RotationIndex++;
            SetRotation();
        }

        counter = 0;
    }

    private void OnMouseDrag()
    {
        if (weapon == null) return;

        if(!dragging) counter += Time.deltaTime;
        if(counter >= dragThreshold)
        {
            if(CrystalManager.Main.buildPoints >= weaponBase.tiles[0].cost)
            {
                inputManager.currentTileInstance = Instantiate(weaponBase.tiles[0], GameObject.FindGameObjectWithTag("MainUI").transform);
                tile = inputManager.currentTileInstance.GetComponent<RectTransform>();
                inputManager.currentTileInstance.ReceiveWeapon(weaponBase);
                dragging = true;
                counter = 0;
            } else
            {
                CrystalManager.Main.BlinkCost();
            }
        }
        if(dragging)
        {
            tile.anchoredPosition = GlobalFunctions.CalculatePointerPosition();
        }
    }

    private void OnMouseEnter()
    {
        if (inputManager.draggingWeapon)
        {
            inputManager.hoveredSlot = gameObject;
        } else if (weaponBase != null)
        {
            WeaponInfoPanel.Main.ReceiveWeapon(weaponBase);
            dragIcon.SetActive(true);
            TutorialManager.Main.RequestTutorialPage(15, 2, true);
        }
    }

    private void OnMouseExit()
    {
        if (inputManager.hoveredSlot == gameObject)
        {
            inputManager.hoveredSlot = null;
        }
        else if (weaponBase != null)
        {
            WeaponInfoPanel.Main.Clear();
            dragIcon.SetActive(false);
        }
    }

    public void ReceiveWeapon(WeaponBase receivedWeapon)
    {
        TutorialManager.Main.RequestTutorialPage(9, 2);

        weapon = receivedWeapon.transform;
        weaponBase = receivedWeapon;
        weapon.SetParent(holder);
        weapon.localPosition = Vector3.zero;
        weapon.rotation = holder.rotation;
        SetRotation();

        receivedWeapon.ApplyPassiveEffect();
    }

    public void SetRotation()
    {
        stationaryAngle = rotations[RotationIndex];
        holder.rotation = Quaternion.Euler(0, 0, stationaryAngle);
    }
}
