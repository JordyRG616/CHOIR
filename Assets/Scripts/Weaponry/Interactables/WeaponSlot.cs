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
    [SerializeField] private float timeToSell;
    [SerializeField] private SpriteRenderer radialBar;
    [Header("Beat Behaviour")]
    [SerializeField] private bool rotating;
    [SerializeField] private bool wandering;
    [SerializeField] private List<Transform> wanderPoints;
    private Material radialMat;
    private float sellingTime;
    private RectTransform tile;
    private float counter;
    private bool draggingTile, draggingWeapon;
    private float stationaryAngle;
    private bool pointerIsInside;
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

    private int _wanderIndex = 0;
    private int WanderIndex
    {
        get => _wanderIndex;
        set
        {
            if (value >= wanderPoints.Count) _wanderIndex = 0;
            else _wanderIndex = value;
        }
    }


    private void Start()
    {
        inputManager = InputManager.Main;
        stationaryAngle = holder.eulerAngles.z;
        StartCoroutine(ManageSelectableVFX());

        radialMat = new Material(radialBar.material);
        radialBar.material = radialMat;

        if(rotating) ActionMarker.Main.OnBeat += SetRotation;
        if(wandering) 
        {
            holder.position = wanderPoints[0].position;
            ActionMarker.Main.OnBeat += Wander;
        }
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
        if(draggingTile)
        {
            Cursor.visible = true;
            if (inputManager.currentHoveredBox != null && !inputManager.currentTileInstance.IsOverReseter)
            {
                inputManager.SetActionTile();
                CrystalManager.Main.ExpendBuildPoints(weaponBase.tile.cost);
            }
            else
            {
                Destroy(tile.gameObject);
            }
            
            draggingTile = false;
        }

        if(draggingWeapon)
        {
            Cursor.visible = true;
            if (inputManager.draggingWeapon && inputManager.hoveredSlot != null)
            {
                inputManager.hoveredSlot.ReceiveWeapon(weaponBase);
                weaponBase = null;
                weapon = null;
            }
            else
            {
                weapon.localPosition = Vector3.zero;
            }

            inputManager.draggingWeapon = false;
        }

        counter = 0;
    }

    private void DoInteraction()
    {
        switch(inputManager.interactionMode)
        {
            case InteractionMode.Default:
                if(!rotating) SetRotation();
            break;
            case InteractionMode.Build:
                if(!IsOccupied)
                {
                    ShopManager.Main.OpenNewWeaponPanel();
                    inputManager.selectedSlot = this;
                }
            break;
            case InteractionMode.Upgrade:
                if(IsOccupied)
                {
                    weaponBase.LevelUp();
                }
            break;
            case InteractionMode.Sell:
                if(IsOccupied)
                {
                    Sell();
                }
            break;
        }

        radialMat.SetFloat("_Fill", sellingTime / timeToSell);
    }

    private void OnMouseDrag()
    {
        if (weapon == null || inputManager.interactionMode != InteractionMode.Default) return;

        if(!draggingTile) counter += Time.deltaTime;
        if(counter >= dragThreshold)
        {
            if(inputManager.interactionMode == InteractionMode.Default)
            {
                CreateTile();
            }
            if(inputManager.interactionMode == InteractionMode.Default)
            {
                inputManager.draggingWeapon = true;
                Cursor.visible = false;
            }
        }
        if (draggingTile)
        {
            tile.anchoredPosition = GlobalFunctions.CalculatePointerPosition();
        }
        if(inputManager.draggingWeapon)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            weapon.position = pos;
        }
    }

    private void CreateTile()
    {
        if (CrystalManager.Main.buildPoints >= weaponBase.tile.cost)
        {
            inputManager.currentTileInstance = Instantiate(weaponBase.tile, GameObject.FindGameObjectWithTag("MainUI").transform);
            tile = inputManager.currentTileInstance.GetComponent<RectTransform>();
            inputManager.currentTileInstance.ReceiveWeapon(weaponBase);
            draggingTile = true;
            counter = 0;
        }
        else
        {
            CrystalManager.Main.BlinkCost();
        }
    }

    private void OnMouseEnter()
    {
        pointerIsInside = true;

        if (inputManager.draggingWeapon && weaponBase == null)
        {
            inputManager.hoveredSlot = this;
        }
        else if (weaponBase != null)
        {
            WeaponInfoPanel.Main.ReceiveWeapon(weaponBase);
            dragIcon.SetActive(true);
            TutorialManager.Main.RequestTutorialPage(15, 2, true);
        }
    }

    private void OnMouseExit()
    {
        pointerIsInside = false;

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

    void Update()
    {
        if(pointerIsInside)
        {
            if(Input.GetMouseButton(0))
            {
                sellingTime += Time.deltaTime;
                radialMat.SetFloat("_Fill", sellingTime / timeToSell);

                if(sellingTime >= timeToSell)
                {
                    DoInteraction();
                }

            } else if(Input.GetMouseButtonUp(0))
            {
                sellingTime = 0;
                radialMat.SetFloat("_Fill", sellingTime / timeToSell);
            }
        }
    }

    private void Sell()
    {
        weaponBase.Sell();
        weaponBase = null;
        weapon = null;
        sellingTime = 0;
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

        receivedWeapon.Set();
    }

    public void SetRotation()
    {
        RotationIndex++;
        stationaryAngle = rotations[RotationIndex];
        holder.rotation = Quaternion.Euler(0, 0, stationaryAngle);
    }

    private void Wander()
    {
        WanderIndex++;
        transform.position = wanderPoints[WanderIndex].position;
    }
}
