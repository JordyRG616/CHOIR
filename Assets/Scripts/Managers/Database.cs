
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Upgrade Database", fileName ="Upgrade Database")]
public class Database : ScriptableObject
{
    [field:SerializeField] public List<WeaponBase> weapons;
    private List<WeaponBase> removedWeapons = new List<WeaponBase>();

    public List<WeaponBase> GetRandomWeapons(int count)
    {
        var list = new List<WeaponBase>();

        for (int i = 0; i < count; i++)
        {
            var rdm = Random.Range(0, weapons.Count);
            var weapon = weapons[rdm];
            list.Add(weapon);
            weapons.Remove(weapon);
        }

        weapons.AddRange(list);

        return list;
    }

    internal void RemoveWeapon(WeaponBase weapon)
    {
        weapons.Remove(weapon);
        removedWeapons.Add(weapon);
    }
}
