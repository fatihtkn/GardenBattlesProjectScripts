using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Animations;

public class PlantShootController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] AimConstraint aimConstraint;

    public ZombieBrain Enemy { get; set; }
    public PlantDataController plantDataController;
    public PlantType plantType;

    bool isTargetChanged;

 
    private void Start()
    {
     
       plantDataController=transform.parent.GetComponent<NewStack>().PlantDataController;   
  
    }
  
   
    public void CreateProjectile(ZombieBrain enemy)
    {
        if (enemy != null)
        {
            Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            projectile.Shoot(enemy, plantDataController.Data.damage);
            
        }
       

    }
    public void CreateProjectile(ZombieBrain enemy,Action<ZombieBrain> hitEvent)
    {
        if (enemy != null)
        {
            Vector3 offset = new(0, -90, 0);
            Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position,Quaternion.identity );
           
            projectile.Shoot(enemy, plantDataController.Data.damage,hitEvent);
            

        }


    }

   

    public void SetConstraint(Transform other)
    {
        if(aimConstraint != null)
        {
            isTargetChanged = true;

            ConstraintSource source = new()
            {
                sourceTransform = other,
                weight = 1f,

            };
            aimConstraint.weight = 0f;
            aimConstraint.SetSource(0, source);
           
            DOTween.To(() => aimConstraint.weight, x => aimConstraint.weight = x, 1, 0.4f);
        }
        
    }
    public void ResetConstraints(Transform resetTransform)
    {
        
        isTargetChanged = false;
        GameObject temp = new("Sa");
        
        temp.transform.SetPositionAndRotation(resetTransform.position, resetTransform.rotation);
        temp.transform.localScale = resetTransform.localScale;
      
        ConstraintSource source = new(){ sourceTransform = temp.transform, weight = 1 };

        aimConstraint.SetSource(0, source);

        DOTween.To(() => aimConstraint.weight, x => aimConstraint.weight = x, 0f, 0.3f).OnUpdate(() => 
        { 
            if (isTargetChanged) 
            {
                
                DOTween.Kill(1);
                if (temp != null) { Destroy(temp.transform.gameObject); }
               
            }
        }).OnComplete(() =>
        {
            if(temp!=null) 
            {
                
                Destroy(temp.transform.gameObject); 
            }
            
        }).id=1;

    }
    public void ResetConstraints(Transform resetTransform,Action canAttack)//this method using by tank type plants
    {
        
        isTargetChanged = false;
        GameObject temp = resetTransform.gameObject;

         temp.transform.SetPositionAndRotation(resetTransform.position, resetTransform.rotation);
         temp.transform.localScale = resetTransform.localScale;
       
        ConstraintSource source = new() { sourceTransform = temp.transform, weight = 1 };

        aimConstraint.SetSource(0, source);

        DOTween.To(() => aimConstraint.weight, x => aimConstraint.weight = x, 0f, 0.25f)
       .OnComplete(() =>
       {
           if (temp != null)
           {
               canAttack.Invoke();
                   
               //Destroy(temp.transform.gameObject);
           }

       }).id = 6;

    }
    
    public enum PlantType
    {
        Ranged,
        Melee,
        Sup
    }
}
