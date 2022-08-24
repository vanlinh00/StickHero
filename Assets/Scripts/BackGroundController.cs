using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : Singleton<BackGroundController>
{
    [SerializeField] GameObject _backGround;

    [SerializeField] float _timeMove;

    protected override void Awake()
    {
        base.Awake();
    }
   public void BornNewBackGround()
    { 
         Vector3 lastPosChild = transform.GetChild(transform.childCount - 1).position;
        Vector3 newPosChild = new Vector3(lastPosChild.x + 18.99258f, lastPosChild.y, lastPosChild.z);
        GameObject newBackGround = ObjectPooler._instance.SpawnFromPool("Bg_1", newPosChild, Quaternion.identity);
        newBackGround.transform.parent = transform;
    }
   public void MoveToLeft()
    {
        Vector3 newPostCamera = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
        StartCoroutine(Move(transform, newPostCamera, _timeMove));
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
