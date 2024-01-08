using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoSingleton<MergeManager>
{
    [SerializeField] GameObject maxLevelTextPrefab;
    public bool Merge(NewStack tileStack, NewStack grabbedStack)
    {
        if (!CanMerge(tileStack, grabbedStack))
        {
            return false;
        }

        bool isMergeSucceeded = TryMerge(tileStack, grabbedStack);

        return isMergeSucceeded;
    }

    private bool CanMerge(NewStack tileStack, NewStack grabbedStack)
    {
        return grabbedStack != null && tileStack != null && tileStack != grabbedStack;
    }

    private bool TryMerge(NewStack tileStack, NewStack grabbedStack)
    {
        var tilePlantLevel = tileStack.PlantDataController.PlantLevel;
        var grabbedPlantLevel = grabbedStack.PlantDataController.PlantLevel;
        var tilePlantType = tileStack.PlantDataController.Data.plantType;
        var grabbedPlantType = grabbedStack.PlantDataController.Data.plantType;

        bool canMerge = tilePlantLevel == grabbedPlantLevel && tilePlantType == grabbedPlantType;
        bool isInitialLevel = ControlInitialLevelException(tilePlantLevel,grabbedPlantLevel);
        bool arePlantsMaxLevel = tileStack.PlantDataController.IsReachedMaxLevel & grabbedStack.PlantDataController.IsReachedMaxLevel;

        if (canMerge)
        {
            if (arePlantsMaxLevel) { NotifyOnMaxLevel(tileStack); return false; }
            
            PerformMergeActions(tileStack, grabbedStack);
        }
       
        else if(isInitialLevel)
        {
            PerformMergeActions(tileStack, grabbedStack);
            canMerge = true;
        }
        

        return canMerge;
    }

    private void PerformMergeActions(NewStack tileStack, NewStack grabbedStack)
    {
       
        tileStack.Bloom();
        SkillManager.Instance.InheritSkillWhileMerge(tileStack, grabbedStack);
        grabbedStack.currentTile = null;
        Destroy(grabbedStack.gameObject);
    }
    void NotifyOnMaxLevel(NewStack tileStack)
    {
        GameObject text = Instantiate(maxLevelTextPrefab, tileStack.transform.position+new Vector3(0,2f,0), Quaternion.identity);
        float targetPos = text.transform.position.y + 3f;
        text.transform.DOMoveY(targetPos, 0.5f).OnComplete(() =>
        {
            Destroy(text);
        });
    }
    bool ControlInitialLevelException(int a, int b)
    {
        if(a == 0 && b == 0) return true;

        return false;
    }

}


