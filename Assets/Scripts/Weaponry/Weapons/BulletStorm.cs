using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStorm : WeaponBase
{
    [SerializeField] private Transform guideShooter;
    [SerializeField] private List<GameObject> perkObjs;

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange += Vector2.one * 2;
            break;
            case 3:
                var inh = MainShooter.inheritVelocity;
                inh.curve = new ParticleSystem.MinMaxCurve(10);
            break;
            case 4:
                damageRange += Vector2.one * 3;
            break;
            case 5:
                var emission = MainShooter.emission;
                var burst = emission.GetBurst(0);
                burst.count = new ParticleSystem.MinMaxCurve(5);
                emission.SetBurst(0, burst);
            break;
        }
    }

    public override void Shoot(WeaponKey key)
    {
        guideShooter.localRotation = Quaternion.Euler(0, 0, guideShooter.localEulerAngles.z + 45);
        base.Shoot(key);
    }

    public override string WeaponDescription()
    {
        return "Shoots 3 pellets in 4 directions. Switches directions each activation. Each pellet deals " + damageRange.x + " - " + damageRange.y + " damage to enemies.";
    }

    protected override void ApplyPerk()
    {
        perkObjs.ForEach(x => x.SetActive(true));
    }

    protected override void RemovePerk()
    {
        perkObjs.ForEach(x => x.SetActive(false));
    }
}
