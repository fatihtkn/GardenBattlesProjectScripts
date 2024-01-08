using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkillDoor : MonoBehaviour
{
    private int skillStack;
    public int maxSkillStack=2;
    public SkillSet desiredSkill;
    public TMP_Text skillPointText;
    private void Start()
    {
        skillPointText.SetText(maxSkillStack.ToString());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack"))
        {
            NewStack stack=other.GetComponent<NewStack>();
            if(stack != null&skillStack<maxSkillStack)
            {
                if(stack.Plant!=null)
                {
                    if (!stack.Plant.HasSkill)
                    {
                        AddSkill(stack.Plant);
                        SkillManager.Instance.CreateSnowFlakeIcon(stack);
                        //stack.skillSprite.SetActive(true);

                        skillStack++;
                        skillPointText.SetText((maxSkillStack - skillStack).ToString());
                    }
                   
                }
            }
        }
    }
    public void AddSkill(Plant plant)
    {
        switch (desiredSkill)
        {
            case SkillSet.Freezing:
                plant.AddComponent<Freeze>();
                plant.InitializeSkill();
                break;
            case SkillSet.Burning:
                plant.AddComponent<Burn>();
                plant.InitializeSkill();
                break;

        }
    }
    
    public enum SkillSet
    {
        Freezing,
        Burning
    }
}
