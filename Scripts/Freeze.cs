using System.Collections;
using UnityEngine;

public class Freeze : Skill
{
    private float freezeValue;
    private SkinnedMeshRenderer zombieMesh;
    private Coroutine freezeCoroutine;

    public override void CastSkill(ZombieBrain enemy)
    {
        if (enemy != null && !enemy.IsFrozen)
        {
            ApplyFreezeEffect(enemy);
        }
    }

    private void ApplyFreezeEffect(ZombieBrain enemy)
    {
        enemy.FreezeValue += 0.25f;
        zombieMesh = enemy.GetZombieRenderer();
        zombieMesh.material.SetFloat("_IceAmount", enemy.FreezeValue);

        float freezeSpeed = enemy.Agent.speed / (enemy.FreezeValue * 10f);
        SlowEnemy(enemy, freezeSpeed);
    }

    private void SlowEnemy(ZombieBrain enemy, float freezeSpeed)
    {
        enemy.Agent.speed -= freezeSpeed;
        freezeCoroutine = StartCoroutine(FreezeEnemyForSecond(enemy, 1.25f));
    }

    private IEnumerator FreezeEnemyForSecond(ZombieBrain enemy, float freezeTime)
    {
        if (enemy.FreezeValue == 1f)
        {
            enemy.DeathEvent += OnTargetKilled;
            enemy.Frost();

            yield return new WaitForSeconds(freezeTime);

            if (enemy != null)
            {
                enemy.DeFrost();
            }
        }
    }

    private void OnTargetKilled()
    {
        StopFreezeCoroutine();
    }

    private void StopFreezeCoroutine()
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
        }
    }
}
