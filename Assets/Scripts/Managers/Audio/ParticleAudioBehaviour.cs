using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ParticleAudioBehaviour : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter OnEmitSFX;
    private ParticleSystem associatedEffect;
    private int cachedCount;

    void Start()
    {
        associatedEffect = GetComponent<ParticleSystem>();
    }

    private void ManageSFX()
    {
        if (associatedEffect.isPlaying)
        {
            if (associatedEffect.particleCount > cachedCount)
            {
                PlaySFX();
            }

            cachedCount = associatedEffect.particleCount;
        }
    }

    private void PlaySFX()
    {
        OnEmitSFX.Play();
    }

    void LateUpdate()
    {
        ManageSFX();
    }
}
