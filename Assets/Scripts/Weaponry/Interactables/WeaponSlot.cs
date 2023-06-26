using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [field:SerializeField] public bool surface {get; private set;}
    [field: SerializeField] public Transform holder { get; private set; }
    [SerializeField] private float dragThreshold = 0.25f;
    [SerializeField] private GameObject dragIcon;
    [SerializeField] private float timeToSell;
    [SerializeField] private SpriteRenderer radialBar;

    [Header("Effects")]
    [SerializeField] private ParticleSystem selectableVFX;
    [SerializeField] private Animator visualAnimator;
    [SerializeField] private ParticleSystem levelUpVFX;
    public ParticleSystem buildVFX;

    [Header("Beat Behaviour")]
    [SerializeField] private bool rotating;
    [SerializeField] private List<int> rotations;
    [SerializeField] private bool wandering;
    [SerializeField] private List<Transform> wanderPoints;
    private Material radialMat;
    private float interactionTime;
    private bool interacted;
    private RectTransform tile;
    private float dragCounter;
    private bool draggingTile, draggingWeapon;
    private float stationaryAngle;
    private bool pointerIsInside;
    public Transform weapon { get; private set; }
    public WeaponBase weaponBase { get; private set; }
    private InputManager inputManager;
    private CrystalManager crystalManager;
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
    public int CurrentRotation => rotations[RotationIndex];
    
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

    private FMOD.Studio.EventInstance chargeInstance;


    private void Start()
    {
        crystalManager = CrystalManager.Main;

        inputManager = InputManager.Main;
        stationaryAngle = holder.eulerAngles.z;

        radialMat = new Material(radialBar.material);
        radialBar.material = radialMat;

        if(rotating) ActionMarker.Main.OnBeat += SetRotation;
        if(wandering) 
        {
            holder.position = wanderPoints[0].position;
            ActionMarker.Main.OnBeat += Wander;
        }

        chargeInstance = AudioManager.Main.RequestInstance(FixedAudioEvent.Charge);

        StartCoroutine(ManageSelectableEffect());
    }

    private IEnumerator ManageSelectableEffect()
    {
        while(true)
        {
            yield return new WaitUntil(() => HighlightCursor(out var c, out var d));

            // selectableVFX.Play();
            visualAnimator.SetBool("Open", true);

            yield return new WaitUntil(() => !HighlightCursor(out var c, out var d));

            // selectableVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            visualAnimator.SetBool("Open", false);
        }
    }

    private void OnMouseUp()
    {
        if(draggingTile)
        {
            Cursor.visible = true;
            if (inputManager.CanPlaceTile())
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
        else if(draggingWeapon)
        {
            Cursor.visible = true;
            if (inputManager.draggingWeapon && inputManager.hoveredSlot != null)
            {
                inputManager.hoveredSlot.ReceiveWeapon(weaponBase, false);
                weaponBase = null;
                weapon = null;
            }
            else
            {
                weapon.localPosition = Vector3.zero;
            }

            draggingWeapon = false;
            inputManager.draggingWeapon = false;
        }
                
        interacted = false;
        dragCounter = 0;
    }

    private void DoInteraction()
    {
        switch(inputManager.interactionMode)
        {
            case InteractionMode.Build:
                if(ShopManager.Main.canBuild)
                {
                    inputManager.selectedSlot = this;
                    ShopManager.Main.OpenNewWeaponPanel();
                    AudioManager.Main.RequestEvent(FixedAudioEvent.Build);
                } else crystalManager.BlinkCost();
            break;
            case InteractionMode.Upgrade:
                if(crystalManager.buildPoints >= weaponBase.level + 1)
                {
                    UpgradeWeapon();
                }
                else crystalManager.BlinkCost();
            break;
            case InteractionMode.Sell:
                    Sell();
            break;
        }

        inputManager.selectedButton.ChangeSelection(true, true);
        interacted = true;
        radialMat.SetFloat("_Fill", interactionTime / timeToSell);
    }

    private void UpgradeWeapon()
    {
        crystalManager.ExpendBuildPoints(weaponBase.level + 1);
        weaponBase.LevelUp();
        WeaponInfoPanel.Main.UpdateInfo();
        levelUpVFX.Play();
        AudioManager.Main.RequestEvent(FixedAudioEvent.Upgrade);
    }

    private void OnMouseDrag()
    {
        if (weapon == null || CanInteract() || interacted) return;

        if(!draggingTile) dragCounter += Time.deltaTime;
        if(dragCounter >= dragThreshold)
        {
            if(inputManager.interactionMode == InteractionMode.Default)
            {
                CreateTile();
            }
            if(inputManager.interactionMode == InteractionMode.Move)
            {
                inputManager.draggingWeapon = true;
                draggingWeapon = true;
                Cursor.visible = false;
            }
        }
        if (draggingTile)
        {
            tile.anchoredPosition = GlobalFunctions.CalculatePointerPosition();
            var image = tile.GetComponent<UnityEngine.UI.Image>();
            if(inputManager.CanPlaceTile()) image.color = Color.white;
            else image.color = Color.red;
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
        if (crystalManager.buildPoints >= weaponBase.tile.cost)
        {
            inputManager.currentTileInstance = Instantiate(weaponBase.tile, GameObject.FindGameObjectWithTag("MainUI").transform);
            tile = inputManager.currentTileInstance.GetComponent<RectTransform>();
            inputManager.currentTileInstance.ReceiveWeapon(weaponBase);
            draggingTile = true;
            dragCounter = 0;
        }
        else
        {
            crystalManager.BlinkCost();
        }
    }

    private void OnMouseEnter()
    {
        pointerIsInside = true;
        SpecialCursor.Main.SetAvailable(HighlightCursor(out var cost, out var desc), cost, desc);
        // selectableVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (inputManager.draggingWeapon && weaponBase == null)
        {
            inputManager.hoveredSlot = this;
        }
        else if (weaponBase != null)
        {
            WeaponInfoPanel.Main.ReceiveWeapon(weaponBase);
            if(inputManager.interactionMode == InteractionMode.Default) dragIcon.SetActive(true);
        }

        if(CanInteract()) AudioManager.Main.RequestEvent(FixedAudioEvent.HoverSlot);
    }

    private void OnMouseExit()
    {
        pointerIsInside = false;
        interactionTime = 0;
        interacted = false;
        SpecialCursor.Main.SetAvailable(false);
        // if(CanInteract()) selectableVFX.Play();

        if (inputManager.hoveredSlot == gameObject)
        {
            inputManager.hoveredSlot = null;
        }
        else if (weaponBase != null)
        {
            WeaponInfoPanel.Main.Clear();
            dragIcon.SetActive(false);
        }
        
        if(inputManager.draggingWeapon) AudioManager.Main.RequestEvent(FixedAudioEvent.TakeWeapon);
    }

    void Update()
    {
        if(pointerIsInside)
        {
            if(Input.GetMouseButtonDown(0) && CanInteract() && !interacted)
            {
                chargeInstance.start();
            }

            if(Input.GetMouseButton(0) && CanInteract() && !interacted)
            {
                interactionTime += Time.deltaTime;
                radialMat.SetFloat("_Fill", interactionTime / timeToSell);

                if(interactionTime >= timeToSell)
                {
                    DoInteraction();
                    interactionTime = 0;
                }

            } else if(Input.GetMouseButtonUp(0))
            {
                interactionTime = 0;
                chargeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            SetRotation(Mathf.RoundToInt(-Input.mouseScrollDelta.y));
        }
        radialMat.SetFloat("_Fill", interactionTime / timeToSell);
    }

    private bool CanInteract()
    {
        if(inputManager.interactionMode == InteractionMode.Build)
        {
            if(IsOccupied) return false;
            else return true;
        } 
        else if(inputManager.interactionMode == InteractionMode.Move) 
        {
            return false;
        }
        else if(inputManager.interactionMode == InteractionMode.Upgrade) 
        {
            if(IsOccupied && !weaponBase.maxLevel) return true;
            return false;
        }
        else if(inputManager.interactionMode == InteractionMode.Sell)
        {
            if(IsOccupied) return true;
            else return false;
        }
        return false;
    }

    private bool HighlightCursor(out int cost, out string desc)
    {
        cost = 0;
        desc = "";

        if(inputManager.interactionMode == InteractionMode.Build)
        {
            if(IsOccupied) return false;
            else 
            {
                cost = ShopManager.Main.weaponCost;
                return true;
            }
        } 
        else if(inputManager.interactionMode == InteractionMode.Move) 
        {
            if(IsOccupied) return true;
            else return false;
        }
        else if(inputManager.interactionMode == InteractionMode.Upgrade) 
        {
            if(IsOccupied && !weaponBase.maxLevel) 
            {
                cost = weaponBase.level + 1;
                desc = weaponBase.nextLevelDescription;
                return true;
            }
            return false;
        }
        else if(inputManager.interactionMode == InteractionMode.Sell)
        {
            if(IsOccupied) return true;
            else return false;
        }
        return false;
    }

    private void Sell()
    {
        weaponBase.Sell();
        weaponBase = null;
        weapon = null;
        AudioManager.Main.RequestEvent(FixedAudioEvent.Sell);
        ShopManager.Main.weaponsSold++;
    }

    public void ReceiveWeapon(WeaponBase receivedWeapon, bool setWeapon = true)
    {
        weapon = receivedWeapon.transform;
        weaponBase = receivedWeapon;
        weapon.SetParent(holder);
        weapon.localPosition = Vector3.zero;
        weapon.rotation = holder.rotation;
        // SetRotation();
        AudioManager.Main.RequestEvent(FixedAudioEvent.PlaceWeapon);

        receivedWeapon.Set(this, setWeapon);
    }

    public void SetRotation()
    {
        RotationIndex++;
        stationaryAngle = rotations[RotationIndex];
        holder.rotation = Quaternion.Euler(0, 0, stationaryAngle);
    }

    public void SetRotation(int direction)
    {
        stationaryAngle = holder.eulerAngles.z + (10 * direction);
        holder.rotation = Quaternion.Euler(0, 0, stationaryAngle);
    }

    private void Wander()
    {
        WanderIndex++;
        transform.position = wanderPoints[WanderIndex].position;
    }
}
