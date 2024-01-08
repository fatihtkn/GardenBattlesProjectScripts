using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ZombieBrain target;
    [SerializeField] private Vector3 projectileHeightOffset;
    [SerializeField] private bool isShooting;

    private bool canShoot;
    private float timer = 0f;
    private float projectileDamage;
    private Action<ZombieBrain> hitEvent;

    public float projectileSpeed;
    public float duration = 0.3f;
    public Vector3 offset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HandleEnemyCollision();
        }
    }

    private void Update()
    {
        if (canShoot && target != null)
        {
            MoveTowardsTarget();
        }

        CheckTargetDeath();
    }

    public void Shoot(ZombieBrain target, float damage, Action<ZombieBrain> hitEvent = null)
    {
        InitializeProjectile(target, damage, hitEvent);
    }

    public void Shoot(ZombieBrain target, float damage)
    {
        InitializeProjectile(target, damage);
    }

    private void InitializeProjectile(ZombieBrain target, float damage, Action<ZombieBrain> hitEvent = null)
    {
        this.hitEvent = hitEvent;
        canShoot = true;
        this.target = target;
        projectileDamage = damage;
        isShooting = true;
        target.DeathEvent += SelfDestroyOnTargetIsDeath;
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + projectileHeightOffset, timer / duration);
        timer += Time.deltaTime;
    }

    private void CheckTargetDeath()
    {
        if (target.isDead && isShooting)
        {
            Destroy(gameObject);
        }
    }

    private void HandleEnemyCollision()
    {
        if (target != null)
        {
            hitEvent?.Invoke(target);
            bool isDead = target.TakeDamage(projectileDamage);
            if (isDead)
            {
                canShoot = false;
                target = null;
            }
        }
        Destroy(gameObject);
    }

    private void SelfDestroyOnTargetIsDeath()
    {
        Destroy(gameObject);
        target = null;
    }

    private void OnDisable()
    {
        if (target != null)
        {
            target.DeathEvent -= SelfDestroyOnTargetIsDeath;
        }
    }
}
