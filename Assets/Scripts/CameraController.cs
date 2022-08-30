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

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _hero = GameObject.FindGameObjectWithTag("Player");
    }
    public void FllowToPlayer()
    {
        UpdatePositionHero();
        Vector3 newPositionCamera = new Vector3(_camera.transform.position.x + _currentPosXHero - _oldPosXHero, _camera.transform.position.y, _camera.transform.position.z);
        StartCoroutine(Move(_camera.transform, newPositionCamera, _timeMove));
    }
    public void SetCameraGHToGP()
    {
        Vector3 newPositionCamera = new Vector3(1.58f, 0, -10);
        StartCoroutine(Move(_camera.transform, newPositionCamera, 0.32f));
    }
    public void Shake()
    {
        StartCoroutine(FadeShake());
    }
    IEnumerator FadeShake()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.06f, transform.position.z);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
        }
    }
    public void SetCameraGPTOGP()
    {
        Vector3 newPositionCamera = new Vector3(1.58f, 0, -10);
        _camera.transform.position = newPositionCamera;
    }
    private void UpdatePositionHero()
    {
        _hero = GameObject.FindGameObjectWithTag("Player");
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
