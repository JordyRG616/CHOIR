using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnCollider : WeaponEffect
{
    [SerializeField] private GameObject spawnable;
    [SerializeField] private bool spawnAsChild;
    [SerializeField] private float lifetime;
    private WaitForSeconds waitLifetime;

    protected override void Start()
    {
        base.Start();
        waitLifetime = new WaitForSeconds(lifetime);
    }

    protected override void Effect(GameObject enemy)
    {
        var container = Instantiate(spawnable, enemy.transform.position, Quaternion.identity);
        if (spawnAsChild) container.transform.SetParent(enemy.transform);

        if (container.TryGetComponent<WeaponDamageDealer>(out var _d)) _d.SetWeapon(damageDealer.GetWeapon());
        container.SetActive(true);

        StartCoroutine(Lifetime(container));
    }

    private IEnumerator Lifetime(GameObject container)
    {
        yield return waitLifetime;

        Destroy(container);
    }

    public float RaiseDuration(float value)
    {
        lifetime += value;
        waitLifetime = new WaitForSeconds(lifetime);
        return lifetime;
    }
}
