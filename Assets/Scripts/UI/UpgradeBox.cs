using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradeBox : MonoBehaviour
{
    // [SerializeField] private Image icon;
    // [SerializeField] private TextMeshProUGUI amount;
    // private UpgradeBase cachedUpgrade;
    // private InputManager pointerHandler;
    // private List<WeaponSlot> slots = new List<WeaponSlot>();
    // private RectTransform upgradeCursor;

    // private void Start()
    // {
    //     Debug.Log("box");
    //     pointerHandler = InputManager.Main;
    // }

    // public void ReceiveUpgrade(UpgradeBase upgrade)
    // {
    //     cachedUpgrade = upgrade;
    //     icon.sprite = upgrade.icon;
    //     amount.text = "x" + upgrade.amount;
    // }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //         Debug.Log("Clicked");
    //     if (ShopManager.Main.HasEnoughPoints(cachedUpgrade.cost))
    //     {
    //     }
    // }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     WeaponInfoPanel.Main.ReceiveUpgrade(cachedUpgrade);
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     WeaponInfoPanel.Main.Clear();
    // }

    // private IEnumerator HighlightSlots()
    // {
        

    //     yield return new WaitUntil(() => pointerHandler.selectingUpgrade == false);

    //     pointerHandler.uiOpen = true;
    //     var weapon = pointerHandler.selectedSlot.weapon.GetComponent<WeaponBase>();
    //     // weapon.FindUpgrade(cachedUpgrade.tag).Apply();
    //     CrystalManager.Main.ExpendBuildPoints(cachedUpgrade.cost);

    //     if (cachedUpgrade.amount > 0) amount.text = "x" + cachedUpgrade.amount;
    //     else Destroy(gameObject);
            

    // }

    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     if (pointerHandler.hoveredSlot != null)
    //     {
    //         var weapon = pointerHandler.hoveredSlot.GetComponent<WeaponSlot>().weaponBase;
    //         // weapon.FindUpgrade(cachedUpgrade.tag).Apply();
    //         Inventory.Main.ExpendUpgrade(cachedUpgrade);

    //         if (cachedUpgrade.amount > 0) amount.text = "x" + cachedUpgrade.amount;
    //         else Destroy(gameObject);
    //     }

    //     foreach (var slot in slots)
    //     {
    //         slot.StopSelectableEffect();
    //     }

    //     upgradeCursor.anchoredPosition = Vector2.one * 750;
    //     Cursor.visible = true;
    //     pointerHandler.selectingUpgrade = false;
    //     pointerHandler.uiOpen = true;
    // }

    // public void OnBeginDrag(PointerEventData eventData)
    // {
    //     slots = FindObjectsOfType<WeaponSlot>().ToList();
    //     slots = slots.FindAll(x => x.IsOccupied);
    //     foreach (var slot in slots)
    //     {
    //         slot.PlaySelectableEffect();
    //     }

    //     upgradeCursor = GameObject.FindGameObjectWithTag("UpgradeCursor").GetComponent<RectTransform>();
    //     Cursor.visible = false;
    //     pointerHandler.selectingUpgrade = true;
    //     pointerHandler.uiOpen = false;
    // }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     upgradeCursor.anchoredPosition = GlobalFunctions.CalculatePointerPosition();
    // }
}
