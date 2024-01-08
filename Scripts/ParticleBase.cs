using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleBase : MonoBehaviour
{
    //public abstract void PlayParticle(Vector3 pos,Transform parent);

    public virtual void PlayParticle( Transform parent)
    {

    }
    public virtual void PlayParticle(Vector3 pos)
    {

    }
}
