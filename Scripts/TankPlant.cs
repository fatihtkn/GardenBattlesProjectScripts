using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TankPlant : Plant,IHealable
{
  
    public bool canAttack;
    [SerializeField]Animator animator;
   public ZombieBrain enemy;
    Action<Transform> OnEnemyPushed;
    Action AttackStateEvent;

    private GameObject constraintReferenceObject;

    Coroutine enemyPushCoroutine;
    [SerializeField] bool isCoroutineStoppedTEST;

    private void Awake()
    {
        plantType = PlantType.Tank;
        constraintReferenceObject = new GameObject("ConstraintReference");
       
    }
    private new void Start()
    {
        base.Start();
        canAttack = true;
        animator = GetComponent<Animator>();
        OnEnemyPushed += ResetConstraints;
        AttackStateEvent += SetAttackState;

       
    }
   
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy")&canAttack)
        {
            if (other.transform.parent.TryGetComponent(out enemy))
            {
                if (enemy.zombieState != ZombieBrain.ZombieStates.Pushing)
                {
                    SetReferenceConstraintObject();
                    ShootController.SetConstraint(enemy.transform);
                    enemy.DeathEvent += OnEnemyDead;
                    PlayAtackAnim();
                    canAttack = false;
                    
                }
                
            }

        }
    }
    public void PushTheEnemy()//Animation Event
    {
        if (enemy != null && !canAttack)
        {
            HandleEnemyPush();
        }
        else
        {
            HandleNoEnemy();
        }
    }

    private void HandleEnemyPush()
    {
        if (enemy.zombieState != ZombieBrain.ZombieStates.Pushing)
        {
            enemyPushCoroutine = StartCoroutine(enemy.Push(OnEnemyPushed, PlantDataController.Data.damage, SkillEffect));
        }
        else
        {
            SetReferenceConstraintObject();
            ShootController.ResetConstraints(constraintReferenceObject.transform, AttackStateEvent);
        }
    }

    private void HandleNoEnemy()
    {
        if (enemy == null)
        {
            ShootController.ResetConstraints(constraintReferenceObject.transform, AttackStateEvent);
        }
    }


    private void SkillEffect(ZombieBrain brain)
    {
        if (plantSkill != null) { plantSkill.CastSkill(enemy); };
    }

    private void ResetConstraints(Transform enemyTransform)
    {
        SetReferenceConstraintObject();

        ShootController.ResetConstraints(constraintReferenceObject.transform,AttackStateEvent);
       

    }
    private void SetReferenceConstraintObject()
    {
        if(enemy!= null)
        {
            constraintReferenceObject.transform.SetPositionAndRotation(enemy._transform.position, enemy._transform.rotation);
            constraintReferenceObject.transform.localScale = enemy._transform.localScale;
        }
      
    }
    private void PlayAtackAnim()
    {
        animator.ResetTrigger("Atack");
        animator.SetTrigger("Atack");

    }

    private void OnEnemyDead()
    {
       
        if (!canAttack)
        {

            SetReferenceConstraintObject();
            ShootController.ResetConstraints(constraintReferenceObject.transform, AttackStateEvent);
        }

    }

    private void SetAttackState()
    {
        if (!canAttack) { canAttack = true; }

        enemy = null;
    }

    public void GetHeal(float healAmount)
    {
        float maxHealth = PlantDataController.Data.maxHealth;
        if (CurrentHealth < maxHealth)
        {
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth,0f, maxHealth);
           
        }
    }
   
    private void OnDisable()
    {
        Destroy(constraintReferenceObject);
    }
   
}
