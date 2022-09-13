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
    [SerializeField] ColumnG2 _nextColumn;
    [SerializeField] ColumnG2 _firstColumn;

    public bool _isStickPill = false;
    private int _currentScore = 0;
    float _angleRotaion = 0f;
    bool _isTouchCol = false;
    int _countTurn = 0;

    private int _countCurrentLemon;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _countTurn = 0;
        BackGroundController._instance._currentPosXHero = _firstColumn.HeaderPosX() + 0.301f;
        Vector3 NewPosHero = new Vector3(_firstColumn.HeaderPosX() + 0.301f, _firstColumn.HeaderPosY() + 0.092534f, 0);
        HerMoveToTarget(NewPosHero);

        _countCurrentLemon = DataPlayer.getInforPlayer().amountMelon;
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
            _nextColumn = columnsManager.GetNextColum().GetComponent<ColumnG2>();
            Vector3 CrossPoint = CalculerCrossPoint();
            _angleRotaion = _stick.FindAngleRotaion(CrossPoint);
            _stick.UpdateRatation(_angleRotaion);
            // _stick.isStickSPill = true;
            _stick.Spill();
            _isStickPill = true;
            StartCoroutine(HeroSPill());
        }   
    }

   Vector3 CalculerCrossPoint()
    {
        Vector3 CrossPoint = new Vector3();
        // find crossPoint
        float PosX = _nextColumn.linearEquations();
        float PosY = _stick.crossPointBetweenCircleAndLinear(PosX);

        if (PosY == 0)
        {
            CrossPoint = new Vector3(PosX, 0, 0);
        }
        else
        {
            if (PosY >= _nextColumn.HeaderPosY())
            {
                if (_stick.transform.position.y > _nextColumn.HeaderPosY())
                {
                    if (_stick.R() > calculerBCWithPyTago())
                    {
                        _isTouchCol = true;

                        CrossPoint = new Vector3(PosX, _nextColumn.HeaderPosY(), 0);
                    }
                    else
                    {
                        CrossPoint = new Vector3(PosX, 0, 0);
                    }
                }
                else
                {
                    _isTouchCol = true;
                    CrossPoint = new Vector3(PosX, _nextColumn.HeaderPosY(), 0);
                }
             }
            else
            {
                _isTouchCol = true;

                CrossPoint = new Vector3(PosX, PosY, 0);
            }
        }
        return CrossPoint;
    }
    IEnumerator HeroSPill()
    {
        yield return new WaitForSeconds(0.09059631417f);
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

        if (!_isTouchCol && MelonG2Manger._instance.IsEnoughMelon())
        {
            _countTurn = 0;

            if (MelonG2Manger._instance.GetCountTouchMelon() == 2)
            {
                SoundManager._instance.OnPlayAudio(SoundType.perfect);
                GamePlayG2._instance.EnablePerfectTxt();
            }

            MelonG2Manger._instance.ResetCountTouchMelon();
            MelonG2Manger._instance.ResetManagerMelon();

            // Hero Move to next col
            Vector3 PosHeadCol = columnsManager.GetPosHeadNextCol();
            Vector3 NewPosHero = new Vector3(PosHeadCol.x+ 0.141f, PosHeadCol.y + 0.092534f, 0);

            _stick.DisnableStick();
            _hero._isMove = true;
            _hero.UpdateMoveMent(NewPosHero, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _hero._isMove = false;
            _stick.EnableStick();
            _isStickPill = false;

            GamePlayG2._instance.UpdateScore(1);
            AudioManager._instance.OnPlayAudio(SoundType.score);
            CameraController._instance.FllowToPlayer();
            BackGroundController._instance.FllowPlayer();
            yield return new WaitForSeconds(0.3f);
            columnsManager.BornNewColumn(_hero.transform.position);
            yield return new WaitForSeconds(1f);
            GamePlayG2._instance.DisablePerfectTxt();
            BornBackGroundFromObjectPool();
        }
        else
        {  
            _countTurn++;
            Debug.Log(_countTurn);

            if (_countTurn < 2)
            {
                _stick.DisnableStick();
                _hero.StateDance();
                yield return new WaitForSeconds(1f);
                _hero.StateIdle();
                _stick.EnableStick();
                _isStickPill = false;
                yield return new WaitForSeconds(0.2f);

                if (MelonG2Manger._instance.CountMelonInCase() == 1)
                {
                    melonManger.GetCurrentMelon().GetComponent<MelonG2>().EnableMelonAgain();
                    MelonG2Manger._instance.ResetCountTouchMelon();
                }
                else
                { 
                    if (_isTouchCol)
                    {
                        MelonG2Manger._instance.LoadMelonAgain();
                    }
                    else
                    {
                        MelonG2Manger._instance.LoadMelonAgainCaseIsTouchColFalse();
                    }
                }

            }
            else
            {
                _hero.MoveDown();
                AudioManager._instance.OnPlayAudio(SoundType.dead);
                CameraController._instance.Shake();
                yield return new WaitForSeconds(0.8f);
                UIControllerG2._instance.EnableGameOver();
                GameOverG2._instance.SetCurrentScore();
            }
            
        }
        _isTouchCol = false;
    }
    void HerMoveToTarget(Vector3 NewPosHero)
    {
        StartCoroutine(FadeHeroMove(NewPosHero));
    }
    IEnumerator FadeHeroMove(Vector3 NewPosHero)
    {
        _stick.DisnableStick();
        _hero._isMove = true;
        _hero.UpdateMoveMent(NewPosHero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        _hero._isMove = false;
        _stick.ResetStick();     // set active = false thi lam sao ma Reset duoc nua
        yield return new WaitForSeconds(0.1f);
        _stick.EnableStick();
    }
    float calculerBCWithPyTago()
    {
        //float AB = Mathf.Abs(_stick.transform.position.y) - Mathf.Abs(_nextColumn.HeaderPosY());
        //float AC = Mathf.Abs(_stick.transform.position.x) - Mathf.Abs(_nextColumn.HeaderPosX());
        //return Mathf.Sqrt(AB * AB + AC * AC);

       Vector3 HeaderCol = new Vector3 (_nextColumn.HeaderPosX(), _nextColumn.HeaderPosY(), 0);
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

    public void SetCountCurrentLemon(int AmountLemon)
    {
        _countCurrentLemon += AmountLemon;
        DataPlayer.UpdateAmountHero(_countCurrentLemon);
    }
    public int GetCountCurrentLemon()
    {
        return _countCurrentLemon;
    }

}
