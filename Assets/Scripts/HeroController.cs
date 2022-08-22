using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : Singleton<HeroController>
{
    [SerializeField] float _moveSpeed;

   public void MoveDown()
    {  
        Vector3 Target = new Vector3(transform.position.x, transform.position.y-5f, 0);
        StartCoroutine(Move(transform, Target, 1f));
    }

    public void MoveByX(float x)
    {  
          Vector3 Target = new Vector3(transform.position.x+x, transform.position.y, 0);
         StartCoroutine(Move(transform, Target, 0.5f));
    }

    public void MoveToPoint(Vector3 Target)
    {
        StartCoroutine(Move(transform, Target, 1f));
    }

    IEnumerator Move(Transform CurrentTransform, Vector3 Target, float TotalTime)
    {
        var passed = 0f;
        var init = CurrentTransform.transform.position;
        while (passed < TotalTime)
        {
            passed += Time.deltaTime;
            var normalized = passed / TotalTime;
            var current = Vector3.Lerp(init, Target, normalized* _moveSpeed);
            CurrentTransform.position = current;
            yield return null;
        }
    }
}
