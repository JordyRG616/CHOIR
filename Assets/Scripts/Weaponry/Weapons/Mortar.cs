using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : WeaponBase
{
    [SerializeField] private Bomb bomb;
    [SerializeField] private float launchForce;
    private bool raiseSize;
    private bool unlockMultishot;

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 5;
            break;
            case 3:
                launchForce += 7.5f;
            break;
            case 4:
                damageRange.x += 3;
            break;
            case 5:
                raiseSize = true;
            break;
        }
    }

    public void DeployBomb()
    {
        var _b = Instantiate(bomb, transform.position, Quaternion.identity);
        var body = _b.GetComponent<Rigidbody2D>();
        var direction = -transform.right * launchForce;
        body.AddForce(direction, ForceMode2D.Impulse);
        _b.Initiate(this, raiseSize, unlockMultishot);
    }

    public override string WeaponDescription()
    {
        return "Shoots a bomb that will explode after three beats, dealing " + damageRange.x + " - " + damageRange.y + " damage and applying <color=red>burn</color> to all enemies in range.";
    }

    protected override void ApplyPerk()
    {
        unlockMultishot = true;
    }

    protected override void RemovePerk()
    {
        unlockMultishot = false;
    }
}
