using Unity.VisualScripting;
using UnityEngine;

public abstract class Plant : MonoBehaviour,IDamageable
{
    [SerializeField] float _health;
    public  float CurrentHealth { get => _health; set => _health = value; }
    public PlantShootController ShootController { get; set; }
    public PlantDataController PlantDataController { get; set; }
    public int NumberOfTargeting { get; set; }
    private int _maxTargetingCount;
    public int MaxTargeting { get=>_maxTargetingCount;  set=>_maxTargetingCount=value; }
    public PlantType plantType;
    public Skill plantSkill;
    public bool HasSkill => GetComponent<Skill>();

    
    private void Awake()
    {
        GridManager.Instance.OnAllPlantsArranged += InitializeSkill;
    }
    public void Start()
    {
        PlantDataController = transform.parent.GetComponent<NewStack>().PlantDataController;
        CurrentHealth = PlantDataController.Data.maxHealth;
        PlantManager.Instance.AddPlant(this);
        
        if (TryGetComponent(out PlantShootController controller))
        {
            ShootController = controller;
         
        }
    }
    public void Damage(float damage)
    {
        CurrentHealth-=damage;
    }


    public  void InitializeSkill()
    {
        if (TryGetComponent(out Skill skill))
        {
            plantSkill = skill;
        }

    }
    private void OnDisable()
    {
        if (this!=null)
        {
            if (PlantManager.Instance != null)
            {
                PlantManager.Instance.RemovePlant(this);
               
            }
        }
        else
        {
            Debug.LogError("Attempted to remove a null plant reference.");
        }

    }
    public void InheritSkill(bool otherHasSkill)//sonra duzgun yapcaz
    {
        if(otherHasSkill)
        {
          plantSkill=  this.AddComponent<Freeze>();
        }
    }
    

}
public enum PlantType
{
    Default,
    Adc,
    Tank,
    Sup
}

