using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoSingleton<PlantManager>
{
    public List<Plant> Plants = new();


    public List<Plant> HealablePlants=new();
    


    public void AddPlant(Plant plant)
    {
        Plants.Add(plant);
       AddHealablePlants(plant);
    }
    public void RemovePlant(Plant plant)
    {
        if (Plants.Contains(plant))
        {
            Plants.Remove(plant);
            RemoveHealablePlants(plant);
        }
    }
    private void AddHealablePlants(Plant plant)
    {
        if (plant is IHealable)
        {
            HealablePlants.Add(plant);
        }

    }
    private void RemoveHealablePlants(Plant plant)
    {
        if (plant is IHealable)
        {
            HealablePlants.Remove(plant);
        }
    }
}
