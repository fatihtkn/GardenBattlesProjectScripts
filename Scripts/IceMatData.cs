using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="IceMaterialData")]
public class IceMatData : ScriptableObject
{
    [field: SerializeField] public float Metallic { get; private set; }
    [field: SerializeField] public float Smooth { get; private set; }
    [field: SerializeField] public float NormalStrenght { get; private set; }



}
