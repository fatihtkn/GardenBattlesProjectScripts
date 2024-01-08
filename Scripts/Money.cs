using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Money : MonoBehaviour
{
    Vector3 canvasTargetPos;
    public Vector3 startScale;
    private void Start()
    {
        startScale = transform.localScale;
    }

    public void CreateMoney(Vector3 enemyDeathPos)
    {
        canvasTargetPos = MoneyManager.Instance.currencyIconTransform.position;
        Vector3 worldPosToCanvas=Camera.main.WorldToScreenPoint(enemyDeathPos);
        transform.position = worldPosToCanvas;
        
        StartCoroutine(StartMoneyInterpolation(canvasTargetPos));

    }

    private IEnumerator StartMoneyInterpolation(Vector3 targetPos)
    {
        float timer = 0f;
        float duration = 0.3f;
        Vector3 startPos = transform.position;
        Vector3 startScale = new(3.7578f, 3.7578f, 3.7578f);
        transform.localScale = startScale;
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            float delta=timer/duration;
            float time = Easing.InOutCubic(delta);

            transform.position=Vector3.Lerp(startPos, targetPos, time);
            transform.localScale=Vector3.Lerp(startScale,Vector3.zero, delta);    
            timer += Time.deltaTime;
            yield return null;
        }

    }


}
