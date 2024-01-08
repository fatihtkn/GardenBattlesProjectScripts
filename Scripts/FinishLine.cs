using System.Collections;          
using System.Collections.Generic;
using UnityEngine;
public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NewStack>(out var stack))
        {
            if(stack.isInitialStack)
            {
                GameManager.Instance.OnGameFinished();
            }
        }
    }

}
