using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroG2 : MonoBehaviour
{
    [SerializeField] float _smoothRotation;
    [SerializeField] float _currentDegree;
    [SerializeField] float _targetDegree;
    public void Spill()
    {
        StartCoroutine(FadeRotation(_currentDegree, _targetDegree));
    }

    IEnumerator FadeRotation(float currentDegree, float targetDegree)
    {
        float t = currentDegree;
        while (t >= targetDegree)
        {
            yield return new WaitForEndOfFrame();
            t -= _smoothRotation;
            Quaternion target = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (t > targetDegree) ? t : targetDegree);
            transform.rotation = target;
        }
    }
    public void MoveToTarget(Vector3 Target,float TimeMove)
    {
        StartCoroutine(Move(transform, Target, TimeMove));
    }
    IEnumerator Move(Transform CurrentTransform, Vector3 Target, float TotalTime)
    {
        var passed = 0f;
        var init = CurrentTransform.transform.position;
        while (passed < TotalTime)
        {
            passed += Time.deltaTime;
            var normalized = passed / TotalTime;
            var current = Vector3.Lerp(init, Target, normalized);
            CurrentTransform.position = current;
            yield return null;
        }

    }
    public void ResetHero()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
