using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportPlant : Plant
{
    [SerializeField] float _healAmount;
    [SerializeField] float _healInterval;
    Animator animator;
    public List<Plant> plants;
   

    private void Awake()
    {
        animator = GetComponent<Animator>();
        plantType = PlantType.Sup;
    }
    private new void Start()
    {
        base.Start();
        StartCoroutine(HealEveryDedicatedSeconds(_healInterval));
    }
   
    private IEnumerator HealEveryDedicatedSeconds(float loop)
    {
        while (true)
        {
            animator.SetBool("Heal",false);
            yield return new WaitForSeconds(loop);
            RestorePlantHealts();
            animator.SetBool("Heal", true);
        }
    }
   
    private void RestorePlantHealts()
    {
        List<Plant> healablePlants = PlantManager.Instance.HealablePlants;
       foreach (IHealable plant in healablePlants)
       {
            plant.GetHeal(_healAmount);
           
       }
    }

    
}
