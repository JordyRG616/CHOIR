using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponBox : MonoBehaviour
{
    // [SerializeField] private Image icon;

    // private WeaponBase weapon;
    // private InputManager pointerHandler;
    // public bool placed;


    // private void Start()
    // {
    //     pointerHandler = InputManager.Main;
    // }

    // public void ReceiveWeapon(WeaponBase weapon)
    // {
    //     this.weapon = Instantiate(weapon, Vector3.one * 500, Quaternion.identity);
    //     this.weapon.gameObject.name = weapon.gameObject.name;
    //     icon.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
    // }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     pos.z = 0;
    //     weapon.transform.position = pos;
    // }

    // public void OnBeginDrag(PointerEventData eventData)
    // {
    //     weapon.gameObject.SetActive(true);
    //     weapon.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
    //     pointerHandler.draggingWeapon = true;
    //     pointerHandler.waitingForSlot = true;
    //     pointerHandler.uiOpen = false;
    // }

    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     if(pointerHandler.hoveredSlot == null)
    //     {
    //         weapon.gameObject.SetActive(false);
    //         weapon.transform.position = Vector3.one * 500;
    //     } else
    //     {
    //         var slot = pointerHandler.hoveredSlot.GetComponent<WeaponSlot>();
    //         slot.ReceiveWeapon(weapon);
    //         weapon.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    //         Destroy(gameObject);
    //     }
    //     pointerHandler.draggingWeapon = false;
    //     pointerHandler.waitingForSlot = false;
    //     pointerHandler.uiOpen = true;
    // }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     WeaponInfoPanel.Main.ReceiveWeapon(weapon, this);
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     WeaponInfoPanel.Main.Clear();
    // }

}
