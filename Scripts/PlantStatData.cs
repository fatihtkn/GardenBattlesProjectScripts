using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStatData : ScriptableObject
{
  
    public List<PlantData> plantLevelDatas = new();


	public PlantData UpdatePlantStats(int plantLevel)
	{
		return plantLevelDatas[plantLevel-1];

	}

}
