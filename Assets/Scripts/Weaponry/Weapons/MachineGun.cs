using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MachineGun : WeaponBase
{
    [SerializeField] private StudioEventEmitter emitter;
    [SerializeField] private float firerate;
    private string knockbackAmount = "slightly";

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange += Vector2.one * 2;
            break;
            case 3:
                damageRange.y += 3;
            break;
            case 4:
                IncreaseKnockback(150);
                knockbackAmount = "moderately";
            break;
            case 5:
                firerate /= 2;
            break;
        }
    }

    private void IncreaseKnockback(float amount)
    {
        var coll = MainShooter.collision;
        coll.colliderForce += amount;
    }

    public override void Shoot(WeaponKey key)
    {
        base.Shoot(key);
        StopAllCoroutines();
        shooting = true;
        anim.SetBool("Shooting", true);
        StartCoroutine(ManageShooting());
    }

    private IEnumerator ManageShooting()
    {
        while(shooting)
        {
            MainShooter.Emit(1);
            emitter.Play();

            yield return new WaitForSeconds(firerate);
        }
    }

    public override void Stop()
    {
        shooting = false;
        anim.SetBool("Shooting", false);
        base.Stop();
    }

    public override string WeaponDescription()
    {
        return "While active, shoots a bullet each " + firerate + " secondes that deals " + damageRange.x + " - " + damageRange.y + " damage each, knocking back the target " + knockbackAmount + ".";
    }

    protected override void ApplyPerk()
    {
        firerate /= 2;
    }

    protected override void RemovePerk()
    {
        firerate *= 2;
    }
}
