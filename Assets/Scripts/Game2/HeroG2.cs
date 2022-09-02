using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroG2 : MonoBehaviour
{
    [SerializeField] float _smoothRotation;
    [SerializeField] float _currentDegree;
    [SerializeField] float _targetDegree;

    // public Vector3 Target
    private float _animation;

    [SerializeField] Vector2 _oldPos;
    public Vector2 _target;
    public bool _isMove = false;
    private void Update()
    {   
       if(_isMove)
        {
            _animation += Time.deltaTime;
            Debug.Log("_moveSpeed" + _animation);

            _animation = _animation % 5f;
            Debug.Log("_moveSpeed % 5f" + _animation);
            Debug.Log("_moveSpeed / 5f" + _animation / 5f);

            transform.position = MathParabola.Parabola(_oldPos,_target, 1f, (_animation / 5f)*10f);
            if(Vector2.Distance(transform.position,_target)==0f)
            {
                _isMove = false;
            }
        }

    }
    public void Spill()
    {
        StartCoroutine(FadeRotation(_currentDegree, _targetDegree));
    }
    public void UpdateToMovement(Vector2 Target)
    {
        _animation = 10f;
        _oldPos = transform.position;
        _target = Target;
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
       // StartCoroutine(Move(transform, Target, TimeMove));

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
