using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] public Image healthBar;
    bool canTrack;
    Transform targetPos;
    [SerializeField] Vector3 offSet=new(-8.49f,0.56f,29.17f);


    public HealthBarController Initialize(Transform targetTransform)
    {
        HealthBarController controller = Instantiate(gameObject).GetComponent<HealthBarController>();
        controller.targetPos = targetTransform;
        controller.canTrack = true;
        controller.healthBar.fillOrigin = 1;
        controller.healthBar.fillAmount = 1;
        return controller;
    }
    private void Update()
    {
        if(canTrack&targetPos!=null)
        {

            transform.position = targetPos.position + offSet;
        }
    }
}
