using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonSaber : WeaponBase
{
    [SerializeField] private GameObject extraArms;

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.x += 2;
            break;
            case 3:
                criticalChance += .15f;
            break;
            case 4:
                damageRange.y += 5;
            break;
            case 5:
                criticalChance += .25f;
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Creates two ion sabers and spins while active, dealing " + damageRange.x + " - " + damageRange.y + " continuous damage and applying <color=purple>frailty</color>. to enemies in range.";
    }

    protected override void ApplyPerk()
    {
        extraArms.SetActive(true);
    }

    protected override void RemovePerk()
    {
        extraArms.SetActive(false);
    }
}
