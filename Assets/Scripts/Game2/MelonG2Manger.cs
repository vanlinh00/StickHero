using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelonG2Manger : Singleton<MelonG2Manger>
{
    [SerializeField] GameObject _currentMelon;
    [SerializeField] GameObject _currentMelon2=null;

    private int _countTouchMelon = 0;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _countTouchMelon = 0;
    }
    public void BornNewMelon(float AB, float HeightStick, Vector3 PosA)
    {
          GameObject NewMeLon = ObjectPooler._instance.SpawnFromPool("Melon", ObjectPooler._instance.transform.position, Quaternion.identity);
          MelonG2 Melon = NewMeLon.GetComponent<MelonG2>();

          Melon.d = 1.38f * Melon.GetLocaleScale();
          Vector3 RealPosMelon1 = Melon.CaculerPosMelon(AB, HeightStick, PosA);
          Vector3 FakePosMelon1 = new Vector3(RealPosMelon1.x + 4f, RealPosMelon1.y, 0);
          Melon.oldPosition = RealPosMelon1;
          Melon.transform.position = FakePosMelon1;

          ObjectPooler._instance.AddElement("Melon", NewMeLon);
         _currentMelon = NewMeLon;

        if (Random.RandomRange(1, 3) == 1)
        {
            GameObject NewMeLon2 = ObjectPooler._instance.SpawnFromPool("Melon", ObjectPooler._instance.transform.position, Quaternion.identity);
            MelonG2 Melon2 = NewMeLon2.GetComponent<MelonG2>();

            Vector3 RealPosMelon2 = new Vector3();
            Vector3 FakePosMelon2 = new Vector3();

            float AddXValue = Random.RandomRange(0f, 1f);

            if (PosA.y <= RealPosMelon1.y)
            {
                RealPosMelon2 = new Vector3(RealPosMelon1.x - AddXValue, RealPosMelon1.y - Melon.d - 0.5f, 0);
                FakePosMelon2 = new Vector3(RealPosMelon2.x + 4f, RealPosMelon2.y, 0);
            }
            else
            {
                RealPosMelon2 = new Vector3(RealPosMelon1.x - AddXValue, RealPosMelon1.y + Melon.d + 0.5f, 0);
                FakePosMelon2 = new Vector3(RealPosMelon2.x + 4f, RealPosMelon2.y, 0);
            }

            Melon2.oldPosition = RealPosMelon2;
            Melon2.transform.position = FakePosMelon2;

            ObjectPooler._instance.AddElement("Melon", NewMeLon2);
            _currentMelon2 = NewMeLon2;
        }

    }

    public GameObject GetCurrentMelon()
    {
        return _currentMelon;
    }
    public GameObject GetCurrentMelon2()
    {
        return _currentMelon2;
    }
    public void CurrentMelonMoveToTarget(GameObject Melon,float TimeMove)
    {
        Vector3 Target = new Vector3(Melon.transform.position.x - 4f, Melon.transform.position.y, 0);
        StartCoroutine(Move(Melon.transform, Target, TimeMove));
    }
    public int CountMelonInCase()
    {
        if(_currentMelon2!=null)
        {
            return 2;
        }
        return 1;
    }
    public void SetCountTouchMelon()
    {
        _countTouchMelon++;
    }
    public void ResetCountTouchMelon()
    {
        _countTouchMelon = 0;
    }
    public void ResetManagerMelon()
    {
        _currentMelon.GetComponent<MelonG2>().isStickTouch = false;

        if (_currentMelon2 != null)
        {
            _currentMelon2.GetComponent<MelonG2>().isStickTouch = false;
        }
        _currentMelon2 = null;
    }
    public void LoadMelonAgain()
    {
        MelonG2 CurrentMelon = _currentMelon.GetComponent<MelonG2>();
        MelonG2 CurrentMelon2 = _currentMelon2.GetComponent<MelonG2>();

        if (CurrentMelon.isStickTouch)
        {
            CurrentMelon.EnableMelonAgain();
        }
        if (CurrentMelon2.isStickTouch)
        {
            CurrentMelon2.EnableMelonAgain();
        }
        ResetCountTouchMelon();
    }

    public int GetCountTouchMelon()
    {
        return _countTouchMelon;
    }
    public bool IsEnoughMelon()
    {
        if(_countTouchMelon == CountMelonInCase())
        {
            return true;
        }
        return false;
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
    public void LoadMelonAgainCaseIsTouchColFalse()
    {
        MelonG2 CurrentMelon = _currentMelon.GetComponent<MelonG2>();
        MelonG2 CurrentMelon2 = _currentMelon2.GetComponent<MelonG2>();

        if (!CurrentMelon.isStickTouch)
        {
            CurrentMelon.EnableMelonAgain();
        }
        if (!CurrentMelon2.isStickTouch)
        {
            CurrentMelon2.EnableMelonAgain();
        }
    }
    //public void CheckStickTouchMelon(Vector3 I1,float R1)
    //{
    //    _currentMelon.GetComponent<MelonG2>().CircleEquationMelon(I1, R1);

    //    if (_currentMelon2 != null)
    //    {
    //        _currentMelon2.GetComponent<MelonG2>().CircleEquationMelon(I1, R1);
    //    }
    //}

}
