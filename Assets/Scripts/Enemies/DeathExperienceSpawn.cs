using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathExperienceSpawn : MonoBehaviour
{
    private ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        var emission = particles.emission;
        var burst = emission.GetBurst(0);
        var multiplier = FindObjectOfType<PatternUpgradeManager>().expMultiplier;
        var totalMin = Mathf.RoundToInt(burst.count.constantMin * multiplier);
        var totalMax = Mathf.RoundToInt(burst.count.constantMax * multiplier);
        burst.count = new ParticleSystem.MinMaxCurve(totalMin, totalMax);
        emission.SetBurst(0, burst);

        particles.Play();
    }
}
