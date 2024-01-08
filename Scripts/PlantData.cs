using UnityEngine;

[CreateAssetMenu(menuName ="PlantData")]
public class PlantData : ScriptableObject
{
    [SerializeField]private string PlantLevel;
    public GameObject plantPrefab;
    public float fireRate;
    public float fireRange;
    public float damage;
    public float maxTargetingCapacity;
    public float maxHealth;
    public PlantType plantType;
    [SerializeField] float snowflakeYOffset;
    public  float SnowflakeYOffset=>snowflakeYOffset;
}
