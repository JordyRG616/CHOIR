using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool classSet;
    public WeaponClass _class;
    public WeaponSource _source;
    public List<Image> icons;
    public List<SetBonus> bonuses;
    public int count;


    void Start()
    {
        WeaponMasterController.Main.ReceiveSet(this);
    }

    public void ReceiveWeapon()
    {
        icons[count].color = Color.white;

        count++;

        CheckBonuses();
    }

    public void RemoveWeapon()
    {
        count --;

        var color = Color.white;
        color.a = 0.3f;
        icons[count].color = color;

        CheckBonuses();
    }

    private void CheckBonuses()
    {
        foreach(var bonus in bonuses)
        {
            if(!bonus.applied && count >= bonus.requiredCount) 
            {
                bonus.upgrade.Apply();
                bonus.applied = true;
            }

            if(bonus.applied && count < bonus.requiredCount) 
            {
                bonus.upgrade.Remove();
                bonus.applied = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        WeaponInfoPanel.Main.ReceiveSet(bonuses[0], bonuses[1]);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WeaponInfoPanel.Main.Clear();
    }
}

[System.Serializable]
public class SetBonus
{
    public int requiredCount;
    public UpgradeBase upgrade;
    public bool applied;

}
