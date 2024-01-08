using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class ZombieBrain : EnemyBase
{
    public NavMeshAgent Agent { get; set; }
    bool canZombieTracking;
    bool canZombieSelectTarget;
    public bool waveZombieCanAttack;
    public bool IsWaveZombie;
    public Animator animator { get; set;}
    [SerializeField] private float zombieDamage=2f;
    [SerializeField]NewStack targetedStack;
    public ZombieStates zombieState;
    public Rigidbody rb { get; set; }
    float blend;
    public bool isDead;
    public float Health;/* { get;  private set; }*/
    [SerializeField]private float maxHealth;
    [Header("UI")]
    public HealthBarController healthBarController;
    Vector3 customDestination;
    public event Action DeathEvent;
 
    SkinnedMeshRenderer skinnedMesh;
    public Material OriginalMat { get; set; }
    public bool IsStunned { get; set; }
    public float FreezeValue { get; set; }
    public bool IsFrozen { get; set; }
    static MoneyManager moneyManager;

    public Transform _transform=>transform;

    
    
    private void Awake()
    {
        
        animator=GetComponent<Animator>();  
        Agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        Health = maxHealth;
        canZombieSelectTarget = true;
        
    }
    private void Start()
    {
        Agent.isStopped = true;
        healthBarController = healthBarController.Initialize(_transform);
        OnWaveZombieKilled();
        ZombieManager.Instance.AddZombieToTheList(this);
        skinnedMesh=GetComponentInChildren<SkinnedMeshRenderer>();
        OriginalMat = skinnedMesh.material;
        moneyManager = MoneyManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack")&canZombieSelectTarget)
        {
            if(other.TryGetComponent<NewStack>(out var stack))
            {
                waveZombieCanAttack = false;
                stack.OnStackDestroyed += TrackRandomStack;
                canZombieSelectTarget = false;
                TargetThePlant(stack);
            }
           
       
        }
    }
    private void Update()
    {

        if (!IsStunned)
        {
            
            if (canZombieTracking) 
            {
                if(targetedStack != null)
                {
                    Agent.SetDestination(targetedStack.transform.position);
                }
                
                
            }

            else if (waveZombieCanAttack)
            {

                Agent.SetDestination(customDestination);




            }
            if (HasReached() & targetedStack != null)
            {
                Atack();
            }
            SetAnimationWithBlend();
        }

        
    }
   
    private void SetAnimationWithBlend()
    {
        blend=Mathf.Clamp(blend, 0f, 1f);
        if (!Agent.isStopped)
        {
            blend += Time.deltaTime*2f ;

        }
        else
        {

            blend -= Time.deltaTime*2f ;
        }
            animator.SetFloat("Blend", blend);
    }
    private void Atack()
    {

        animator.SetTrigger("Attack");
        zombieState = ZombieStates.Attacking;
        print("ZombieAttackCount");
    }
    private bool HasReached()
    {
        bool control = Agent.remainingDistance <= Agent.stoppingDistance;
        
        return control;
    }
    public void GiveDamage()//AE
    {
        if(zombieState != ZombieStates.Pushing)
        {
            if (targetedStack!= null)
            {
                if (targetedStack.Plant == null)
                {
                    targetedStack.Damage(1f);
                }
                else
                {
                    targetedStack.Plant.Damage(zombieDamage);
                }

            }
           
            
            //print("Plant Damaged");
        }
        else
        {
            //print("NoDamage");
        }
    }

    private void TargetThePlant(NewStack stack)
    {
        if(stack.NumberOfTargeting<5)
        {
           
            Agent.isStopped = false;
            
            zombieState = ZombieStates.Walking;
            targetedStack = stack;
            canZombieTracking=true;
            
            stack.NumberOfTargeting++;
            targetedStack.OnPlantUpdated += UpdateTargetedPlant;
           
        }
        
    }
    private void UpdateTargetedPlant(Plant plant)
    {
        
        targetedStack.Plant = plant;
        
    }
    
    private void OnDisable()
    {
        
        if (targetedStack != null)
        {
           
            targetedStack.OnPlantUpdated -= UpdateTargetedPlant;
            
            targetedStack.NumberOfTargeting--;
        }
    }
    public IEnumerator Push(Action<Transform> action,float damage,Action<ZombieBrain> skillEffect)
    {


        if (this != null)
        {
            if(gameObject.activeSelf)
            {
                Agent.isStopped = true;
                rb.AddForce(_transform.forward * -1 * 50f, ForceMode.Impulse);
                zombieState = ZombieStates.Pushing;
                skillEffect?.Invoke(this);
                TakeDamage(damage);
            }
           
            
        }
        yield return new WaitForSeconds(0.3f);

        if (this != null)
        {
            rb.velocity = Vector3.zero;
            if (!isDead)
            {
                //rb.velocity = Vector3.zero;
                Agent.isStopped = false;
                zombieState = ZombieStates.Walking;
                action.Invoke(_transform);

            }
           
        }
        
    }
    
    public override bool TakeDamage(float damage)
    {
        
        Health -= damage;
        float healthPercentage = Health / maxHealth;
        healthBarController.healthBar.fillAmount = healthPercentage;
        bool isDead=false;
        if(Health <= 0)
        {
           
            isDead = true;
            if (!this.isDead)
            {
                OnZombieDeath();
            }
          
          
        }
        return isDead;
    }

    #region GetClosestPlantLogic
    //private Plant GetClosestPlant()
    //{
    //    Vector3 ourPosition = transform.position;
    //    Plant targetedPlant = null;
    //    float closestDistance = Mathf.Infinity;
    //    List<Plant> plants = PlantManager.Instance.Plants;
    //    foreach (Plant plant in plants)
    //    {
    //        float distanceDelta = Vector3.Distance(ourPosition, plant.transform.position);

    //        if (distanceDelta < closestDistance & plant.IsTargetable)
    //        {
    //            closestDistance = distanceDelta;
    //            plant.IsTargetable = false;
    //            targetedPlant = plant;
    //            //isZombieTargeting = true;

    //        }
    //    }
    //    //null state yaz 
    //    if (targetedPlant == null) { targetedPlant = plants[0]; }

    //    return targetedPlant;
    //}

    #endregion

    public void OnZombieDeath()
    {
        
        moneyManager.StartMoneyInterpolation(_transform.position);

        DeathEvent += Death;
        DeathEvent.Invoke();
       
    }
    
    public void Death()
    {
        if (Health <= 0)
        {
            if(this!= null)
            {
                
                
                if(gameObject.activeSelf)
                {
                   
                    
                    isDead = true;
                    gameObject.SetActive(false);
                    healthBarController.gameObject.SetActive(false);
                  

                }
                
            }
           


        }
    }

    
    public void SetCustomDestination(Vector3 pos)
    {
        Agent.isStopped = false;
        customDestination = pos;
      
    }
   public SkinnedMeshRenderer GetZombieRenderer()
    {
       
        return skinnedMesh;
    }
    
    private void OnWaveZombieKilled()
    {
        if (IsWaveZombie)
        {
            DeathEvent += ZombieManager.Instance.OnWaveZombieDead;
            

        }
      
    }
    private void TrackRandomStack()
    {
        zombieState = ZombieStates.Walking;
        
        canZombieSelectTarget = true;
        targetedStack = null;
      
        while (targetedStack == null)
        {
            targetedStack = NewStackManager.Instance.GetRandomStack();
           
        }
        

      
    }
    public void Frost()
    {
        
        Agent.isStopped = true;
        animator.enabled = false;
        IsStunned = true;
        animator.speed = 0f;
        IsFrozen = true;
    }
    public void DeFrost()
    {
       
        Agent.isStopped = false;
        animator.enabled = true;

        IsStunned = false;
        animator.speed = 1f;

        FreezeValue = 0f;
        IsFrozen = false;
    }
    
    public enum ZombieStates
    {
        Idle,
        Walking,
        Attacking,
        Pushing
    }
}
