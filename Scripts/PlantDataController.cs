using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDataController : MonoBehaviour
{
    public int PlantLevel;
    public List<PlantData> plantLevelDatas = new();// created plant levels
    public PlantData Data { get; set; }
    public bool IsReachedMaxLevel => IsPlantMaxLevel();

    
    private void Start()
    {
        Data = plantLevelDatas[PlantLevel];
    }


    private PlantData UpdatePlantStats(int plantLevel)
    {
        return plantLevelDatas[plantLevel-1];

    }
    public void LevelUpThePlant()
    {
        PlantLevel++;
        Data=UpdatePlantStats(PlantLevel);
        
        
    }
    private bool IsPlantMaxLevel()
    {
        bool control =PlantLevel==plantLevelDatas.Count;
        return control;
    }

}
