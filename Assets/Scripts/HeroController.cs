using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroController : Singleton<HeroController>
{
    [SerializeField] float _timeMove;
    public SpriteRenderer _heroSprite;

    private bool _canClick = true;

    public int countClick = 0;

    public bool isMoveX = false;

    public Vector3 _target= new Vector3(0,0,0);

    public float disTanceWithColX;  

    private float _speed = 3f;

    public enum HeroState
    {
       idle,
       run,
       die,
       live,
    }

    public HeroState heroState;

    protected override void Awake()
    {
        base.Awake();
        heroState = HeroState.live;
    }

    private void Update()
    {
        CheckCantClick();
        if (_canClick == false)
        {
            return;
        }
        if (Input.GetMouseButtonUp(0))
         {
               if(GameManager._instance.isPlaying)
            {
                countClick++;
             }
        }  

        if (isMoveX)
        {
            if (countClick % 2 == 0)
            {
                FlipDown();
            }
            else
            {
                FilpUp();
            }
            //3.3796
            var step = _speed *Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target, step);

            if (DistanceWithPosXLeftCol() <= 0.15f&&_heroSprite.flipY)
            {
                isMoveX=false;
                GameManager._instance.GameOver();
                heroState = HeroState.die;
            }
        }

    }
    // t= 3.3795/ 3
    // t= 3.2 / 3
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
       //StartCoroutine(Move(transform, Target, 0.5f));
        transform.DOMove(Target, 0.5f);
    }

    public void MoveByX(float x)
    {  
          Vector3 Target = new Vector3(transform.position.x+x, transform.position.y, 0);
          StartCoroutine(Move(transform, Target, 0.5f));
       //   transform.DOMove(Target, 0.5f);
    }

    public void MoveToPoint(Vector3 Target)
    {
        // StartCoroutine(Move(transform, Target, 0.5f));
        transform.DOMove(Target, 0.5f);
    }

    public void FlipDown()
    {
          transform.position = new Vector3(transform.position.x, -2.234f, transform.position.z);
         _heroSprite.flipY = true;
    }

    public void FilpUp()
    {
      transform.position = new Vector3(transform.position.x, -1.922364f, transform.position.z);
        _heroSprite.flipY = false;
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

    //IEnumerator Move(Transform startMarker, Vector3 targetPosition, float startTime)
    //{
    //    float speed = 1.0F;
    //    float journeyLength;
    //    startTime = Time.time;
    //    journeyLength = Vector3.Distance(startMarker.position, targetPosition);
    //    float distCovered = (Time.time - startTime) * speed;
    //    float fractionOfJourney = distCovered / journeyLength;
    //    float timeElapsed = 0;
    //    while (timeElapsed < distCovered)
    //    {
    //        timeElapsed += fractionOfJourney;
    //        transform.position = Vector3.Lerp(startMarker.position, targetPosition, fractionOfJourney);
    //        yield return null;
    //    }
    //}

}
