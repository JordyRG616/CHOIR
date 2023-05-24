
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Upgrade Database", fileName ="Upgrade Database")]
public class Database : ScriptableObject
{
    public List<WeaponBase> weapons;
    public List<ModuleBase> modules;
    private List<WeaponBase> removedWeapons = new List<WeaponBase>();
    private List<ModuleBase> removedModules = new List<ModuleBase>();

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

    public void RemoveWeapon(WeaponBase weapon)
    {
        weapons.Remove(weapon);
        removedWeapons.Add(weapon);
    }

    public List<ModuleBase> GetRandomModules(int count)
    {
        var list = new List<ModuleBase>();

        for (int i = 0; i < count; i++)
        {
            var rdm = Random.Range(0, modules.Count);
            var module = modules[rdm];
            list.Add(module);
            modules.Remove(module);
        }

        modules.AddRange(list);

        return list;
    }

    public void RemoveModule(ModuleBase module)
    {
        modules.Remove(module);
        removedModules.Add(module);
    }
}
