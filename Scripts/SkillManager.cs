using Unity.VisualScripting;
using UnityEngine;

public class SkillManager:MonoSingleton<SkillManager>
{
    private  bool HasSkillLeastOne(NewStack first, NewStack second)
    {
       
        if (first.Plant!=null&& second.Plant!=null)
        {
            if (first.Plant.HasSkill || second.Plant.HasSkill) 
            { 
                return true; 
            }
           
        }

        return false;

    }
    public  void InheritSkillWhileMerge(NewStack tileStack, NewStack grabbedStack)
    {
        if (HasSkillLeastOne(tileStack, grabbedStack))
        {
          
            tileStack.Plant.AddComponent<Freeze>();
            tileStack.Plant.InitializeSkill();
            CreateSnowFlakeIcon(tileStack);
        }
    }
    public void CreateSnowFlakeIcon(NewStack stack)
    {
        GameObject snowFlake = Instantiate(MoneyManager.Instance.snowFlakePrefab, stack.transform);
        snowFlake.transform.localPosition = new(0, stack.PlantDataController.Data.SnowflakeYOffset, 0);
    }
}
