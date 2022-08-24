using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : Singleton<HeroController>
{
    [SerializeField] float _timeMove;
    [SerializeField] SpriteRenderer _heroSprite;
    protected override void Awake()
    {
        base.Awake();
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
