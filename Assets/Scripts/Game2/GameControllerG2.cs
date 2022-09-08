using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameControllerG2 : Singleton<GameControllerG2>
{
    [SerializeField] ColumnsManagerG2 columnsManager;
    [SerializeField] MelonG2Manger melonManger;
     
    [SerializeField] HeroG2 _hero;
    [SerializeField] StickG2 _stick;
   
    private bool _isStickPill = false;

    private int _currentScore = 0;
    float _angleRotaion = 0f;
    bool _isTouchCol = false;
    int _countTurn = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (!_isStickPill)
        {
            _stick.GrowUp();
            _stick.GetDown();
        }
        if (Input.GetMouseButtonDown(0)&& !_isStickPill)
        {
            _stick.CaculerStartWidthTrail();

            Vector3 CrossPoint = calculerCrossPoint();

            _angleRotaion = _stick.FindAngleRotaion(CrossPoint);

            _stick.UpdateRatation(_angleRotaion);

            // _stick.isStickSPill = true;

            _stick.Spill();

            _isStickPill = true;

            StartCoroutine(HeroSPill());
        }   
    }

    private void Start()
    {
        BackGroundController._instance._currentPosXHero = _hero.transform.position.x;
    }
   Vector3 calculerCrossPoint()
    {
        // find crossPoint
        float PosX = columnsManager.GetNextColum().GetComponent<ColumnG2>().linearEquations();
        float PosY = _stick.crossPointBetweenCircleAndLinear(PosX);

        Vector3 CrossPoint = new Vector3();

        if (PosY == 0)
        {
            CrossPoint = new Vector3(PosX, 0, 0);
        }
        else
        {
            if (PosY >= columnsManager.GetNextColum().GetComponent<ColumnG2>().HeaderPosY())
            {
                if (_stick.transform.position.y > columnsManager.GetNextColum().GetComponent<ColumnG2>().HeaderPosY())
                {
                    if (_stick.R() > calculerBCWithPyTago())
                    {
                        _isTouchCol = true;

                        CrossPoint = new Vector3(PosX, columnsManager.GetNextColum().GetComponent<ColumnG2>().HeaderPosY(), 0);
                    }
                    else
                    {
                        CrossPoint = new Vector3(PosX, 0, 0);
                    }
                }
                else
                {
                    _isTouchCol = true;

                    CrossPoint = new Vector3(PosX, columnsManager.GetNextColum().GetComponent<ColumnG2>().HeaderPosY(), 0);
                }
            }
            else
            {
                CrossPoint = new Vector3(PosX, PosY, 0);
            }
        }
        return CrossPoint;
    }
    IEnumerator HeroSPill()
    {
        yield return new WaitForSeconds(0.05059631417f);
        _hero.isHeroSpill = true;
        yield return new WaitForSeconds(0.255f);
        //_stick.isStickSPill = false;
        StartCoroutine(CheckStick());
    }
    IEnumerator CheckStick()
    {
        yield return new WaitForSeconds(0.255f);
        _hero.isHeroSpill = false;

        _stick.ResetStick();
        _hero.ResetHero();

        if (_stick._isCollisionMelon&& !_isTouchCol)
        {
            GamePlayG2._instance.UpdateScore(1);
            _currentScore++;

            // Hero Move
            Vector3 PosHeadCol = columnsManager.GetPosHeadNextCol();
            Vector3 NewPosHero = new Vector3(PosHeadCol.x+ 0.141f, PosHeadCol.y + 0.092534f, 0);
            _hero._isMove = true;
            _hero.UpdateMoveMent(NewPosHero, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _hero._isMove = false;

            CameraController._instance.FllowToPlayer();
            BackGroundController._instance.FllowPlayer();

            yield return new WaitForSeconds(0.3f);
            columnsManager.BornNewColumn();
            yield return new WaitForSeconds(0.5f);

            float disStickAndNextCol = columnsManager.GetNextColum().transform.position.x - _hero.transform.position.x;
            Vector3 PosA = new Vector3(columnsManager.GetNextColum().transform.position.x, _hero.transform.position.y, 0);

            melonManger.BornNewMelon(disStickAndNextCol, disStickAndNextCol, PosA);

            _isStickPill = false;
            _countTurn = 0;

            BornBackGroundFromObjectPool();
        }
        else
        {
            _countTurn++;

            if (_countTurn<=1)
            {
                _hero.StateDance();
                yield return new WaitForSeconds(1f);
                _hero.StateIdle();
                _isStickPill = false;
            }
            else
            {
                _hero.MoveDown();
                UIControllerG2._instance.EnableGameOver();
            }
            
        }
        _isTouchCol = false;
        _stick._isCollisionMelon = false;
    }

    float calculerBCWithPyTago()
    {
       //float AB = Mathf.Abs(_stick.transform.position.y) - Mathf.Abs(columnsManager.GetNextColum().GetComponent<ColumnG2>().HeaderPosY());
       //float AC = Mathf.Abs(_stick.transform.position.x) - Mathf.Abs(columnsManager.GetNextColum().GetComponent<ColumnG2>().HeaderPosX());
       //return Mathf.Sqrt(AB * AB + AC * AC);

       Vector3 HeaderCol = new Vector3 (columnsManager.GetNextColum().GetComponent<ColumnG2>().HeaderPosX(), columnsManager.GetNextColum().GetComponent<ColumnG2>().HeaderPosY(), 0);
       float dist = Vector3.Distance(HeaderCol, _stick.transform.position);
       return dist;
    }

    void BornBackGroundFromObjectPool()
    {
        Vector3 PosLastChild = BackGroundController._instance.GetPosXLastChildBGDynamic();
        if (Mathf.Abs(_hero.transform.position.x - PosLastChild.x) < 9f)
        {
            BackGroundController._instance.BornNewBGDynamic();
            GameObject oldBackGround = BackGroundController._instance.OldBackGrounDynamic();
            string tagColumn = "bg_" + BackGroundController._instance.idBg;
            ObjectPooler._instance.AddElement(tagColumn, oldBackGround);
        }
    }
    public void SetCurrentScore(int Score)
    {
        _currentScore++;
    }
    public int GetCurrentScore()
    {
        return _currentScore;
    }
    //public void SetCountCurrentLemon(int AmountLemon)
    //{
    //    _countCurrentLemon += AmountLemon;
    //    DataPlayer.UpdateAmountHero(_countCurrentLemon);
    //}
    //public int GetCountCurrentLemon()
    //{
    //    return _countCurrentLemon;
    //}

}
