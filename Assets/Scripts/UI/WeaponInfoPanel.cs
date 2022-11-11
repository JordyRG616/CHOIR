using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoPanel : MonoBehaviour
{
    [SerializeField] private Sprite inactiveTab;
    [SerializeField] private List<GameObject> tabs, boxes, buttons;

    public void DeactivateAll()
    {
        tabs.ForEach(x => x.GetComponent<Image>().sprite = inactiveTab);
        boxes.ForEach(x => x.SetActive(false));
        buttons.ForEach(x => x.GetComponent<Image>().sprite = inactiveTab);
    }
}
