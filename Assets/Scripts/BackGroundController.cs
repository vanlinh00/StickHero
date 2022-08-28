using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : Singleton<BackGroundController>
{
    [SerializeField] GameObject _backGroundIdle;
    public GameObject _backGrounDynamic;
    [SerializeField] float _timeMove;
    [SerializeField] float _distanceBeweentBg = 18.99258f;
    [SerializeField] float _distanceBeweentBg2 = 18.9f;

    [SerializeField] GameObject _hero;

    private float _currentPosXHero = 0f;
    private float _oldPosXHero = 0f;
    public float idBg = 1;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(ChangeBackGround());
    }
    IEnumerator ChangeBackGround()
    {
        yield return new WaitForEndOfFrame();

        idBg = Random.RandomRange(1, 3);
        BornBgIdle();
        BornNewBGDynamic();

        InforPlayer inforPlayer = DataPlayer.getInforPlayer();
        if (inforPlayer.idLoadGameAgain)
        {
            SetBackGroundGPTOGP();
        }
        // SaveData
        DataPlayer.UpdataLoadGameAgain(false);
    } 
    public void BornBgIdle()
    {
        GameObject bgIdle = Instantiate(Resources.Load("BackGround/bg_idle_"+idBg, typeof(GameObject)),new Vector3(-0.53f, 3.03f, 0),Quaternion.identity) as GameObject;
        bgIdle.transform.parent = _backGroundIdle.transform;
    }
   public void BornNewBGDynamic()
    {
        float distance = (idBg == 1) ? _distanceBeweentBg : _distanceBeweentBg2;
        Vector3 lastPosChild;
        Vector3 newPosChild;
         if (_backGrounDynamic.transform.childCount!=0)
        {
            lastPosChild = _backGrounDynamic.transform.GetChild(_backGrounDynamic.transform.childCount - 1).position;
            newPosChild = new Vector3(lastPosChild.x + distance, lastPosChild.y, lastPosChild.z);
        }
        else
        {
            newPosChild = new Vector3(0f, 1.1f, 0f);
        }
         GameObject newBackGround = ObjectPooler._instance.SpawnFromPool("bg_"+idBg, newPosChild, Quaternion.identity);
        newBackGround.transform.parent = _backGrounDynamic.transform;
    }
   public void MoveToLeft()
    {
         Vector3 newPosBackGround = new Vector3(_backGrounDynamic.transform.position.x - 1f, _backGrounDynamic.transform.position.y, _backGrounDynamic.transform.position.z);
         StartCoroutine(Move(_backGrounDynamic.transform, newPosBackGround, _timeMove));
    }

    public void FllowPlayer()
    {
          UpdatePositionHero();
         Vector3 newPositionBackGrounIdle = new Vector3(_backGroundIdle.transform.position.x + _currentPosXHero - _oldPosXHero, _backGroundIdle.transform.position.y, _backGroundIdle.transform.position.z);
         StartCoroutine(Move(_backGroundIdle.transform, newPositionBackGrounIdle, _timeMove));

       // Vector3 newPositionBackGrounDynamic = new Vector3(_backGrounDynamic.transform.position.x + _currentPosXHero - _oldPosXHero, _backGrounDynamic.transform.position.y, _backGrounDynamic.transform.position.z);
        //StartCoroutine(Move(_backGrounDynamic.transform, newPositionBackGrounDynamic, _timeMove));
    }
    private void UpdatePositionHero()
    {
        _oldPosXHero = _currentPosXHero;
        _currentPosXHero = _hero.transform.position.x;
    }
    public void SetBackGroundGHToGP()
    {
        Vector3 newPositionBackGrounIdle = new Vector3(2.22f, -1.18f, 0f);
        StartCoroutine(Move(_backGroundIdle.transform, newPositionBackGrounIdle, 0.32f));

        Vector3 newPositionBackGroundDy = new Vector3(0, -1.18f, 0);
        StartCoroutine(Move(_backGrounDynamic.transform, newPositionBackGroundDy, 0.32f));
    }

    public void SetBackGroundGPTOGP()
    {
        Vector3 newPositionBackGrounIdle = new Vector3(2.2f, -1.34f, 0f);
        _backGroundIdle.transform.position = newPositionBackGrounIdle;

        Vector3 newPositionBackGroundDy = new Vector3(0, -1.12f, 0);
        _backGrounDynamic.transform.position = newPositionBackGroundDy;
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
