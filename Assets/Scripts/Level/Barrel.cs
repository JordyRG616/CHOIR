using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer barrelRenderer;
    [SerializeField] private Sprite tumbled;
    [SerializeField] private ParticleSystem explosion, spill;
    [SerializeField] private GameObject spillCollider;
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<WeaponDamageDealer>(out var damageDealer))
        {
            var _class = damageDealer.GetWeapon().weaponSource;

            if(_class == WeaponSource.Flame || _class == WeaponSource.Flame) Explode();
            if(_class == WeaponSource.Projectile) StartCoroutine(Tumble());
        }
    }

    private IEnumerator Tumble()
    {
        barrelRenderer.sprite = tumbled;
        yield return new WaitUntil(() => Mathf.Abs(transform.eulerAngles.z) >= 85f);

        barrelRenderer.GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Spill");
        spillCollider.SetActive(true);
        spill.Play();
    }

    private void Explode()
    {
        barrelRenderer.gameObject.SetActive(false);
        explosion.Play();
    }
}
