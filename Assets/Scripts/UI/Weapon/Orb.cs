using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private WeaponDamageDealer damageDealer;
    [SerializeField] private WeaponDamageDealer aoe;

    public void ReceiveWeapon(Warlock warlock)
    {
        damageDealer.SetWeapon(warlock);
        aoe.SetWeapon(warlock);
        if(warlock.levelFive) damageDealer.statuses.Add(StatusType.Static);
        if(warlock.perkApplied) aoe.gameObject.SetActive(true);
        warlock.OnStop.AddListener(() => Destroy(gameObject));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
