using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Collider col;
    private List<NewStack> stacks = new List<NewStack>();

    [Header("Jump")]
    public Transform targetTransform;
    public float jumpPower;
    public int jumpNumber;
    public float jumpDuration;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack"))
        {
            col.enabled = false;
            BreakTheStack();
        }
    }

    private void BreakTheStack()
    {
        stacks = NewStackManager.Instance.stacks;

        foreach (NewStack stack in stacks)
        {
            if (!stack.isInitialStack)
            {
                HandleStackBreaking(stack);
            }
        }
    }

    private void HandleStackBreaking(NewStack stack)
    {
        stack.StackCol.enabled = false;
        stack.Crash();

        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-6f, 6f), 0, 7f);
        Vector3 targetPosition = stack.transform.position + randomOffset;

        stack.transform.DOJump(targetPosition, jumpPower, jumpNumber, jumpDuration).OnComplete(() =>
        {
            stack.StackCol.enabled = true;
            PlantManager.Instance.RemovePlant(stack.Plant);
            NewStackManager.Instance.RemoveStack(stack);
        });
    }
}
