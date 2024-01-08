using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpParticle : ParticleBase
{
    [Header("Level Up Particle  Values")]
    [SerializeField] ParticleSystem _levelUpParticle;
    [SerializeField] float particlePlayDuration;
    [SerializeField] Vector3 levelUpParticleHeightOffset = new(0, 1f, 0);
    // public bool isPlayable { get; set; }
    private void Start()
    {
        //isPlayable = true;
        _levelUpParticle = GetComponent<ParticleSystem>();
    }
    public override void PlayParticle(Transform parent)
    {
        // isPlayable = false;
        _levelUpParticle.transform.parent = parent;
        _levelUpParticle.transform.localPosition = Vector3.zero;
        //_levelUpParticle.transform.position = pos + levelUpParticleHeightOffset;
        _levelUpParticle.Play(true);


    }
    
}
