using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : Singleton<BackGroundController>
{
    [SerializeField] GameObject _hero;
    [SerializeField] GameObject _backGround;

    private float _currentPosXHero = 0f;
    private float _oldPosXHero = 0f;
    [SerializeField] float _timeMove;

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        _currentPosXHero = _hero.transform.position.x;
    }
   public void FllowToPlayer()
    {
        UpdatePositionHero();
        Vector3 newPositionBackGround = new Vector3(_backGround.transform.position.x + _currentPosXHero - _oldPosXHero, _backGround.transform.position.y, _backGround.transform.position.z);
        StartCoroutine(Move(_backGround.transform, newPositionBackGround, _timeMove));
    }

    private void UpdatePositionHero()
    {
        _oldPosXHero = _currentPosXHero;
        _currentPosXHero = _hero.transform.position.x;
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
