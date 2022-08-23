using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] GameObject _hero;
    [SerializeField] GameObject _camera;
    [SerializeField] float _timeMove;

    private float _currentPosXHero = 0f;
    private float _oldPosXHero = 0f;


    // set Camrera  : StartGame : -1.9, 0.86
    protected override void Awake()
    {
        base.Awake();
    }

    public void FllowToPlayer()
    {
        UpdatePositionHero();

        Vector3 newPositionCamera = new Vector3(_camera.transform.position.x + _currentPosXHero - _oldPosXHero, _camera.transform.position.y, _camera.transform.position.z);
        StartCoroutine(Move(_camera.transform, newPositionCamera, _timeMove));

    }
    public void SetCameraGamePlay()
    {
        Vector3 newPositionCamera = new Vector3(0.49f, 0, -10);
        StartCoroutine(Move(_camera.transform, newPositionCamera, 0.32f));
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
