using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathExperienceSpawn : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private ParticleSystem particles;
    private Transform tank;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        tank = CrystalManager.Main.transform;
    }

    private void Start()
    {
        var emission = particles.emission;
        var burst = emission.GetBurst(0);
        //var multiplier = FindObjectOfType<PatternUpgradeManager>().expMultiplier;
        //var totalMin = Mathf.RoundToInt(burst.count.constantMin * multiplier);
        //var totalMax = Mathf.RoundToInt(burst.count.constantMax * multiplier);
        //burst.count = new ParticleSystem.MinMaxCurve(totalMin, totalMax);
        emission.SetBurst(0, burst);

        particles.Play();
    }

    private void Update()
    {
        var direction = (tank.position + Vector3.up) - transform.position;

        transform.position += direction * speed * Time.deltaTime;
        //speed += Time.deltaTime * 10;
    }
}
