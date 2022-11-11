using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Griffin : MonoBehaviour
{
    [SerializeField] private Vector2 walkDurationLimits, flightDurationLimits;
    [SerializeField] private GameObject walkCollider, flyCollider, feet;
    [SerializeField] private float walkHeight, flyHeight;
    [SerializeField] private ParticleSystem shockwave;
    private WaitForSeconds waveDuration;
    private Animator anim;
    private float timer;
    private float counter;
    private bool flying;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        timer = Random.Range(walkDurationLimits.x, walkDurationLimits.y);
        waveDuration = new WaitForSeconds(shockwave.main.startLifetime.constant);

        foreach (var weapon in FindObjectsOfType<PiercingBulletWeapon>())
        {
            weapon.ReceiveCollider(walkCollider.GetComponent<Collider2D>());
            weapon.ReceiveCollider(flyCollider.GetComponent<Collider2D>());
        }
    }

    private void Update()
    {
        counter += Time.deltaTime;

        if (!flying && counter >= timer)
        {
            walkCollider.SetActive(false);
            flyCollider.SetActive(true);
            feet.SetActive(false);
            anim.SetTrigger("Lift");
            flying = true;
            timer = Random.Range(flightDurationLimits.x, flightDurationLimits.y);
            counter = 0;
        }

        if (flying && counter >= timer)
        {
            flyCollider.SetActive(false);
            anim.SetTrigger("Land");
            flying = false;
            timer = Random.Range(walkDurationLimits.x, walkDurationLimits.y);
            counter = 0;
        }
    }

    public void A_DoShockwave()
    {
        shockwave.Play();
        StartCoroutine(ActivateColliders());
    }

    public void A_RaiseHeight()
    {
        transform.localPosition = new Vector3(transform.position.x, flyHeight);
    }

    public void A_LowerHeight()
    {
        transform.localPosition = new Vector3(transform.position.x, walkHeight);
    }

    public IEnumerator ActivateColliders()
    {
        yield return waveDuration;

        walkCollider.SetActive(true);
        feet.SetActive(true);
    }
}
