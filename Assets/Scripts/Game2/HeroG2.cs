using DG.Tweening;
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

    Parabola _parabola;
    Vector3 _startPos;
    private float _preTime;

    public bool isHeroSpill = false;
    private Transform _targetAngle;
    float _timeCount = 0.0f;

    [SerializeField] Animator _animator;
    [SerializeField] StickG2 _stick;
    [SerializeField] GameObject _stickClone;
    [SerializeField] GameObject _hero;

    private void Start()
    {
        _targetAngle = new GameObject().transform;
        _targetAngle.eulerAngles = new Vector3(0, 0, -70);
    }
    private void Update()
    {
        if (_isMove)
        {    
            if (((Time.time - _preTime) / duration) <= 1)
            {
                _parabola.Move(transform, _startPos, _target, (Time.time - _preTime) / duration);
                StateRotate();
            }
            else
            {
                transform.position = _target;
                StateIdle();
            }
        }
        if (isHeroSpill)
        {
            _hero.transform.rotation = Quaternion.Lerp(_hero.transform.rotation, _targetAngle.rotation, _timeCount);
            _timeCount = _timeCount + Time.deltaTime/2;
        }
    }
   public void EnableAnimator(bool res)
    {
        _animator.gameObject.SetActive(res);
    }
   public void StateIdle()
    {
        _stickClone.SetActive(false);

        _animator.SetBool("Dance", false);
        _animator.SetBool("Rotate", false);
    }
    public void StateDance()
    {
        _stickClone.SetActive(true);
        _animator.SetBool("Dance", true);
    }
    public void StateRotate()
    {
        _stickClone.SetActive(true);

        _animator.SetBool("Dance", false);
        _animator.SetBool("Rotate", true);
    }
    public void UpdateMoveMent(Vector3 Target, float Duration)
    {
        _target = Target;
        duration = Duration;
        _preTime = Time.time;
        _startPos = transform.position;
        _parabola = new Parabola(1);
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
    public void MoveDown()
    {
        Vector3 Target = new Vector3(transform.position.x, transform.position.y - 10f, 0);
        transform.DOMove(Target, 0.8f);
    }
    public void ResetHero()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _hero.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
