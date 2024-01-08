using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleManager : MonoSingleton<ParticleManager>
{
    [Header("All Particles Pool Values")]
   // [SerializeField] ParticleBase _hitParticlePrefab;
    [SerializeField] List<ParticleBase> allParticlePool= new();
    [SerializeField] List<ParticleBase> particlesPrefabs = new();
    //[SerializeField] int hitParticlePoolSize;


    private void Start()
    {
        CreateParticles();
    }

    public void PlaySelectedParticle<T>(Vector3 particlePos) where T : ParticleBase
    {
        foreach (var particle in allParticlePool)
        {
            if (particle is T selectedParticle)
            {
                selectedParticle.PlayParticle(particlePos);
            }
        }
    }
    public void PlaySelectedParticle<T>(Transform parent) where T : ParticleBase
    {
        foreach (var particle in allParticlePool)
        {
            if (particle is T selectedParticle)
            {
                selectedParticle.PlayParticle(parent);
            }
        }
    }

    private void CreateParticles()
    {

        for (int i = 0; i < particlesPrefabs.Count; i++)
        {
            allParticlePool.Add(Instantiate(particlesPrefabs[i]));
           
        }

    }



}
