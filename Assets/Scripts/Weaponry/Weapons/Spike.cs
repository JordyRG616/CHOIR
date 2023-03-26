using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : WeaponBase
{
    [SerializeField] private GameObject burst;

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 3;
            break;
            case 3:
                IncreaseRange(4);
            break;
            case 4:
                damageRange.x += 5;
            break;
            case 5:
                IncreaseRange(6.5f);
            break;
        }
    }

    private void IncreaseRange(float value)
    {
        var shape = MainShooter.shape;
        shape.radius = value;
    }

    public void StartBurst()
    {
        MainShooter.Play();
    }

    public override string WeaponDescription()
    {
        return "While active, releases electric discharges from the surface that deals " + damageRange.x + " - " + damageRange.y + " damage and apply <color=yellow>static</color> to enemies in range.";
    }

    protected override void ApplyPerk()
    {
        burst.SetActive(true);
    }

    protected override void RemovePerk()
    {
        burst.SetActive(false);
    }
}
