using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : Singleton<MoveToPlayer>
{
    [SerializeField] GameObject _hero;
    [SerializeField] GameObject _camera;

    [SerializeField] GameObject _backGroundIdle;
    [SerializeField] float _timeMove;

    private float _currentPosXHero = 0f;
    private float _oldPosXHero = 0f;

    protected override void Awake()
    {
        base.Awake();
    }
    public void FllowToPlayer()
    {
        UpdatePositionHero();
        Vector3 newPositionCamera = new Vector3(_camera.transform.position.x + _currentPosXHero - _oldPosXHero, _camera.transform.position.y, _camera.transform.position.z);
        Vector3 newPositionBackGrounIdle = new Vector3(_backGroundIdle.transform.position.x + _currentPosXHero - _oldPosXHero, _backGroundIdle.transform.position.y, _backGroundIdle.transform.position.z);

        StartCoroutine(Move(_camera.transform, newPositionCamera, _timeMove));
        StartCoroutine(Move(_backGroundIdle.transform, newPositionBackGrounIdle, _timeMove));
    }
    public void SetCameraGHToGP()
    {
        Vector3 newPositionCamera = new Vector3(1.58f, 0, -10);
        StartCoroutine(Move(_camera.transform, newPositionCamera, 0.32f));

        Vector3 newPositionBackGrounIdle = new Vector3(1.63f, 0.76f, 0);
        StartCoroutine(Move(_backGroundIdle.transform, newPositionBackGrounIdle, 0.32f));

        Vector3 newPositionBackGround = new Vector3(0, -1.18f, 0);
        StartCoroutine(Move(BackGroundController._instance.transform, newPositionBackGround, 0.32f));
    }
    public void SetCameraGPTOGP()
    {
        Vector3 newPositionCamera = new Vector3(1.58f, 0, -10);
        _camera.transform.position = newPositionCamera;

        Vector3 newPositionBackGrounIdle = new Vector3(1.61f, 0.76f, 0);
        _backGroundIdle.transform.position = newPositionBackGrounIdle;

        Vector3 newPositionBackGround = new Vector3(0, -1.18f, 0);
        BackGroundController._instance.transform.position = newPositionBackGround;
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
