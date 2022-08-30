using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroController : MonoBehaviour
{
    [SerializeField] float _timeMove;

    private bool _canClick = true;

    public int countClick = 0;

    public bool isMoveX = false;

    private Vector3 _target= new Vector3(0,0,0);

    public float disTanceWithColX;  

    private float _speed = 2.4f;

    [SerializeField] Animator _animator;
    public enum HeroState
    {
       idle,
       run,
       die,
       living,
       shrug,
    }

    public HeroState heroState;

    private void Awake()
    {
    
        heroState = HeroState.living;
        StateIdle();
    }
    private void Update()
    {
        CheckCantClick();

        if (_canClick == false)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
         {
              if (GameManager._instance.isPlaying /*&& GameManager._instance._isPlayerOnStick*/) 
            {
                countClick++;
                if(countClick!=1)
                {
                    OnAudioFlip();
                }  
            }
        }
        if (isMoveX)
        {
            if (countClick % 2 == 0 && !GameManager._instance.IsPlayerOnNextCol(transform.position.x)) 
            {
                FlipDown();
            }
            if(countClick % 2 != 0 && !GameManager._instance.IsPlayerOnNextCol(transform.position.x))
            {
                FilpUp();
             }
                var step = _speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _target, step);
            
                if (DistanceWithPosXLeftCol() <= 0.15f && IsFlipY())
                {
                    isMoveX = false;
                    heroState = HeroState.die;
                    GameManager._instance.GameOver();
                }
        }

    }
    void OnAudioFlip()
    {
        AudioManager._instance.OnPlayAudio(SoundType.roll_up_down);
    }
    public void StateIdle()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Shrug", false);
        _animator.SetBool("Kick", false);
    }
    public void StateRun()
    {
        _animator.SetBool("Run", true);
    }
    public void StateShrug()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Kick", false);
        _animator.SetBool("Shrug", true);
    }
    public void StateKick()
    {   
        _animator.SetBool("Run", false);
        _animator.SetBool("Shrug", false);
        _animator.SetBool("Kick", true);    
    }
    // t= 3.3795/ 3
    // t= 3.2 / x
    public void CaculerSpeed()
    { 
       //  with distance = 3.3795 => V= 3
       _speed = 3;
        float DistanceTo = Mathf.Abs(transform.position.x- _target.x);
        float NewSpeed = (DistanceTo / 3.3795f) * _speed;
      _speed = NewSpeed;
    }
    public float CaculerTimeWait()
    {
        float DisHeroToTarget = Mathf.Abs(transform.position.x - _target.x);
        return (DisHeroToTarget / _speed);
    }    
    void CheckCantClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _canClick = false;
            }
            else
            {
                _canClick = true;
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                _canClick = false;
            }
            else
            {
                _canClick = true;
            }
        }
    }
    float DistanceWithPosXLeftCol()
    {
       return Mathf.Abs(transform.position.x - disTanceWithColX);
    }
    public void SetTarget(float x)
    {
           _target = new Vector3(transform.position.x + x, transform.position.y, 0);
    }
    public void MoveDown()
    {  
        Vector3 Target = new Vector3(transform.position.x, transform.position.y-5f, 0);
        transform.DOMove(Target, 0.2f);
    }

    public void MoveToPoint(Vector3 Target)
    {
        transform.DOMove(Target, 0.5f);
    }

    public void FlipDown()
    {
        transform.eulerAngles = new Vector3(0, -180f, -180f);
        transform.position = new Vector3(transform.position.x, -2.289f, transform.position.z);
    }

    public void FilpUp()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.position = new Vector3(transform.position.x, -1.862f, transform.position.z);
    }
    public bool IsFlipY()
    {
         return (transform.rotation.eulerAngles.y == 180f) ? true : false;
    }
    public void UpdateTarget(Vector3 NewTarget)
    {
        _target = NewTarget;
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
}
