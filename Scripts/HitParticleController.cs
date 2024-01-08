using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleController : ParticleBase
{
    [Header("Hit Particle  Values")]
    ParticleSystem _hitParticle;
    [SerializeField] float particlePlayDuration;
    [SerializeField]Vector3 hitParticleHeightOffset=new(0,3f,0);
   // public bool isPlayable { get; set; }
    private void Start()
    {
        //isPlayable = true;
        _hitParticle=GetComponent<ParticleSystem>();
    }
    public override void PlayParticle(Transform parent)
    {
        // isPlayable = false;
        // _hitParticle.transform.position= pos+ hitParticleHeightOffset;
        _hitParticle.transform.parent= parent;
        _hitParticle.transform.localPosition = Vector3.zero+hitParticleHeightOffset;
        _hitParticle.Play(true);


    }
}
