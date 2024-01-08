using DG.Tweening;
using System;
using UnityEngine;

public class NewStack : MonoBehaviour, IDamageable
{
    [Header("Plant")]
    public Transform plantParent;
    public Plant Plant;
    public bool doorLevel;
    public PlantDataController PlantDataController;
    public GameObject soil;

    public bool canMove;
    public Transform TargetPos { get; set; }

    [Header("Tracking")]
    public float smoothTime;
    public float maxSpeed;
    bool isCollected;
    [SerializeField] float trackingOffset = 0.9f;

    [field: SerializeField]
    public int NumberOfTargeting { get; set; }
    public bool isInitialStack;
    public Vector3 gridPosition { get; set; }
    public event Action<Plant> OnPlantUpdated;

    public Tile currentTile;
    public bool isThisStackGrabbed;

    public GameObject skillSprite;
    public event Action OnStackDestroyed;
    public GameObject constraintRefernceObject;
    Transform _transform;
    float health = 5;
    public Collider StackCol { get; set; }

    private void Awake()
    {
        canMove = true;
        SetInitialStack();
        constraintRefernceObject = new GameObject("Plant Constraint Object");
        _transform = transform;
        StackCol = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack") && !isCollected)
        {
            NewStackManager.Instance.AddStack(this);
        }
    }

    private void Update()
    {
        if (isCollected && !isInitialStack && canMove)
        {
            TrackWithInterpolation();
        }
    }

    public void Bloom()
    {
        InititalizePlant();
    }

    private void InititalizePlant()
    {
        if (PlantDataController.PlantLevel > 0 && Plant != null)
        {
            PlantManager.Instance.RemovePlant(Plant);
            Destroy(Plant.gameObject);
        }

        ParticleManager.Instance.PlaySelectedParticle<LevelUpParticle>(transform);
        PlantDataController.LevelUpThePlant();
        GameObject plant = Instantiate(PlantDataController.Data.plantPrefab, transform);
        Plant = plant.GetComponent<Plant>();

        plant.transform.position = plantParent.position;
        OnPlantLevelUp();
    }

    private void OnPlantLevelUp()
    {
        OnPlantUpdated?.Invoke(Plant);
    }

    public void SetTrackableTarget(Transform transform)
    {
        TargetPos = transform;
        isCollected = true;
    }

    private void TrackWithInterpolation()
    {
        _transform.DOMoveX(TargetPos.position.x, 0.2f);
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, TargetPos.position.z + trackingOffset);
    }

    private void SetInitialStack()
    {
        if (!isInitialStack) return;

        NewStackManager.Instance.AddStack(this);
        isCollected = true;
    }

    public void Crash()
    {
        canMove = false;
        isCollected = false;
    }

    private void OnDisable()
    {
        if (NewStackManager.Instance != null)
        {
            PlantManager.Instance.RemovePlant(Plant);
            NewStackManager.Instance.RemoveStack(this);
            NewStackManager.Instance.OnAllStacksDestroyed();
            OnStackDestroyed?.Invoke();
            OnPlantUpdated = null;
        }
    }

    public void BuyStack(Tile tile)
    {
        canMove = false;
        isCollected = true;
        currentTile = tile;
    }

    public void Damage(float takenDamage)
    {
        health -= takenDamage;
        if (health <= 0 && this != null)
        {
            Destroy(gameObject);
        }
    }
}
