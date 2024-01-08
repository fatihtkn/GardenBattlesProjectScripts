using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewStackManager : MonoSingleton<NewStackManager>
{
    
    public List<NewStack> stacks = new();

    [field: SerializeField]public int DestroyedStackCount { get; private set; }
   
   
    public void AddStack(NewStack trackObject)
    {
        stacks.Add(trackObject);
        bool isStackInitalized = stacks.Count > 1;
        if (isStackInitalized)
        {
            trackObject.canMove = true;
            trackObject.SetTrackableTarget(stacks[stacks.Count - 2].transform);
        }
        
       

    }
    public void RemoveStack(NewStack stack)
    {
      
       stacks.Remove(stack);
      
        
    }
    public void OnAllStacksDestroyed()
    {

        if(stacks.Count == 0)
        {
            GameManager.Instance.OnGameOver();
           
        }
    }
    
    public NewStack GetRandomStack()
    {
        foreach (NewStack stack in stacks)
        {
            if (stack != null)
            {
                return stack;
            }
        }
        //return null;
       
        return stacks[0/*Random.Range(0, stacks.Count)*/];
    }
    
   
}
