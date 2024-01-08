using UnityEngine;

public class AtackerPlant : Plant,IHealable
{
    
    [SerializeField]ZombieBrain enemy;
    [SerializeField]Animator animator;
    [SerializeField] bool aimLocked;
    GameObject constraintReferenceObject;
    
    private void Awake()
    {
        plantType = PlantType.Adc;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy")&!aimLocked)
        {
            if(other.transform.parent.TryGetComponent(out ZombieBrain enemy))
            {
                this.enemy = enemy; 
                SetConstraintReferenceObject();
                ShootController.SetConstraint(constraintReferenceObject.transform);
                aimLocked = true;
                enemy.DeathEvent += OnEnemyDead;
                PlayAtackAnim();
            }
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")&aimLocked)
        {
            if (enemy == other.transform.parent.GetComponent<ZombieBrain>())
            {
                PlayIdleAnim();
                SetConstraintReferenceObject();
                ShootController.ResetConstraints(constraintReferenceObject.transform);
                enemy = null;
                aimLocked = false;
                if(enemy!=null)enemy.DeathEvent-=OnEnemyDead;
            } 
        }
    }
    private new void Start()
    {
        base.Start();
        constraintReferenceObject = transform.parent.GetComponent<NewStack>().constraintRefernceObject;
    }
    private void ShotTheEnemy()// Animation Event
    {
        void hitEvent(ZombieBrain brain) => OnHitTheTarget(enemy);
        ShootController.CreateProjectile(enemy,hitEvent);
       
    }
    private void OnHitTheTarget(ZombieBrain enemy)
    {
        if (plantSkill != null) 
        { 
            plantSkill.CastSkill(enemy);
            PlayHitParticleEffect();
        }
    }
    private void Update()
    {
        if (enemy != null)
        {
            SetConstraintReferenceObject();
        }
    }
    private void OnEnemyDead()
    {
        
        PlayIdleAnim();
        
        SetConstraintReferenceObject();
        ShootController.ResetConstraints(constraintReferenceObject.transform);
        aimLocked = false;
    }
    
    private void OnDisable()
    {
        if (this != null & enemy != null)
        {
            enemy.DeathEvent -= OnEnemyDead;
           
        }
        
    }
    
    private void PlayAtackAnim()
    {
        animator.ResetTrigger("Atack");
        animator.SetTrigger("Atack");
    }
    private void PlayIdleAnim()
    {
        if(animator!=null)
        {
            animator.ResetTrigger("Atack");
            animator.SetTrigger("Exit");
        }
       
    }

    public void GetHeal(float healAmount)
    {
        float maxHealth = PlantDataController.Data.maxHealth;
        if (CurrentHealth < maxHealth)
        {
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);
            print(this + "Health is= " + CurrentHealth);    

        }
    }
    private void PlayHitParticleEffect()
    {
        if (enemy != null)
        {
            ParticleManager.Instance.PlaySelectedParticle<HitParticleController>(enemy._transform);
        }
        
    }
   
    private void SetConstraintReferenceObject()
    {
        if (enemy != null)
        {
            constraintReferenceObject.transform.SetPositionAndRotation(enemy._transform.position+new Vector3(0,2,0), enemy._transform.rotation);
            constraintReferenceObject.transform.localScale = enemy._transform.localScale;

        }



    }
}
