using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroG2 : MonoBehaviour
{
    [SerializeField] float _smoothRotation;
    [SerializeField] float _currentDegree;
    [SerializeField] float _targetDegree;

    public Vector3 _target;
    public bool _isMove = false;
    public float duration;

    Parabola parabola;
    Vector3 startPos;
    float preTime;

    public bool isHeroSpill = false;
    public Transform a;
    float timeCount = 0.0f;

    [SerializeField] Animator _animator;
    [SerializeField] StickG2 _stick;
    public GameObject stickClone;

    [SerializeField] GameObject _hero;
    private void Update()
    {
        if (_isMove)
        {    
            if (((Time.time - preTime) / duration) <= 1)
            {
                parabola.Move(transform, startPos, _target, (Time.time - preTime) / duration);
            }
            else
            {
                transform.position = _target; 
            }
        }
        if (isHeroSpill)
        {
            _hero.transform.rotation = Quaternion.Lerp(_hero.transform.rotation, a.rotation, timeCount);
            timeCount = timeCount + Time.deltaTime/2;
        }
    }
  public void EnableAnimator(bool res)
    {
        _animator.gameObject.SetActive(res);
    }
   public void StateIdle()
    {
        _animator.SetBool("Dance", false);
        _animator.SetBool("Rotate", false);
    }
    public void StateDance()
    {
        _animator.SetBool("Dance", true);

        //_stickClone.SetActive(true);
        //_stick.gameObject.SetActive(false);
    }
    public void StateRotate()
    {
       // _stick.gameObject.SetActive(false);
        //_stickClone.SetActive(true);

        _animator.SetBool("Dance", false);
        _animator.SetBool("Rotate", true);
    }
    public void UpdateMoveMent(Vector3 Target, float Duration)
    {
        _target = Target;
        duration = Duration;
        preTime = Time.time;
        startPos = transform.position;
        parabola = new Parabola(1);
    }
    //public void Spill()
    //{
    //    StartCoroutine(FadeRotation(_currentDegree, _targetDegree));
    //}
    //IEnumerator FadeRotation(float currentDegree, float targetDegree)
    //{
    //    float t = currentDegree;
    //    while (t >= targetDegree)
    //    {
    //        yield return new WaitForEndOfFrame();
    //        t -= _smoothRotation;
    //        Quaternion target = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (t > targetDegree) ? t : targetDegree);
    //        transform.rotation = target;
    //    }
    //}

    public void ResetHero()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _hero.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
