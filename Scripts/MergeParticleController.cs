using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeParticleController : ParticleBase
{
    [Header("Merge Particle Pool Values")]
    ParticleSystem _mergeParticle;
    [SerializeField] float particlePlayDuration;
    [SerializeField] Vector3 mergeParticleHeightOffset = new(0, 0f, 0);
    // public bool isPlayable { get; set; }
    private void Start()
    {
        //isPlayable = true;
        _mergeParticle = GetComponent<ParticleSystem>();
    }
    public override void PlayParticle( Transform parent)
    {
        // isPlayable = false;
        _mergeParticle.transform.position =  mergeParticleHeightOffset;
        _mergeParticle.Play(true);


    }
}
