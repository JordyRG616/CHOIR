using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warlock : WeaponBase
{
    [SerializeField] private Orb orb;
    [SerializeField] private float launchForce;
    public bool levelFive { get; private set; }

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 5;
            break;
            case 3:
                damageRange.x += 3;
            break;
            case 4:
                damageRange += Vector2.one * 3;
            break;
            case 5:
                levelFive = true;
            break;
        }
    }

    [ContextMenu("Launch")]
    public void Launch()
    {
        var _o = Instantiate(orb, transform.position, Quaternion.identity);
        _o.ReceiveWeapon(this);
        var body = _o.GetComponent<Rigidbody2D>();
        var direction = -transform.right * launchForce;
        body.AddForce(direction, ForceMode2D.Impulse);
    }

    public override string WeaponDescription()
    {
        return "Releases an electric orb that deals " + damageRange.x + " - " + damageRange.y + " damage to any enemy it touches. The orb lasts as long as the weapon is active.";
    }

    protected override void ApplyPerk()
    {
        
    }

    protected override void RemovePerk()
    {
        
    }
}
