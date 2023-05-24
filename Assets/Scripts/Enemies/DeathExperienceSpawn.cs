using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathExperienceSpawn : MonoBehaviour
{
    [SerializeField] private GameObject heal;
    private ParticleSystem particles;
    private Transform tank;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        tank = CrystalManager.Main.transform;
    }

    public void Initialize(Vector3 position, float multiplier)
    {
        multiplier += Inventory.Main.GlobalExpMultiplier;
        var emission = particles.emission;
        var burst = emission.GetBurst(0);
        var totalMin = Mathf.RoundToInt(burst.count.constantMin * multiplier);
        var totalMax = Mathf.RoundToInt(burst.count.constantMax * multiplier);
        burst.count = new ParticleSystem.MinMaxCurve(totalMin, totalMax);
        emission.SetBurst(0, burst);

        transform.position = new Vector3(position.x, position.y, tank.transform.position.z);

        if(ModuleActivationManager.Main.HasSpecialEffect(ModuleSpecialEffect.Regenerative)) heal.SetActive(true);

        particles.Play();
        StartCoroutine(WaitForDeath());
    }

    private IEnumerator WaitForDeath()
    {
        yield return new WaitUntil(() => particles.particleCount == 0);

        Destroy(gameObject);
    }
}
