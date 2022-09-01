using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static HeroController;

public class GameManager : Singleton<GameManager>
{
    public GameObject hero;
    private HeroController _heroController;

    [SerializeField] GameObject _currentColumn;
    [SerializeField] GameObject _nextColumn;

    [SerializeField] GameObject _allColumns;
    [SerializeField] GameObject _allSticks;
    [SerializeField] GameObject _allFuit;
    
    private Column _currentCol;
    private Column _nextCol;

    private Stick _currentStick;

    public bool isPlaying;
    private bool _canClick = true;

    private int _currentSore;
    private int _countCurrentLemon;

    public bool isPlayerOnStick = false;
    private bool _isStick_grow_loop = false;

    private int _countTimeTut = 0;

    protected override void Awake()
    {
        isPlaying = false;
        base.Awake();
    }
    private void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        _heroController = hero.GetComponent<HeroController>();
        _currentSore = 0;
        _countCurrentLemon = DataPlayer.getInforPlayer().amountMelon;
        UpdateValueOfColums();
    }
   public void UpLoadHero(GameObject Hero)
    {
        hero = Hero;
        _heroController = hero.GetComponent<HeroController>();
    }

    private void Update()
    {
        CheckCanClick();

        if (_canClick == false)
        {
            return;
        }
        if (isPlaying)
        {
            if (Input.GetMouseButton(0)&& !isPlayerOnStick)
            {
                _heroController.StateShrug();

                _currentStick.Grow();

                if(!_isStick_grow_loop)
                {
                    EnableSoundStickGrow();

                    _isStick_grow_loop = true;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isStick_grow_loop = false;

                SoundManager._instance.audioFx.loop = false;

                if (!isPlayerOnStick)
                {
                    isPlayerOnStick = true;
                    StartCoroutine(StickSpill());
                }
            }

        }
    }
    void CheckCanClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _canClick = false;
            }
            else
            {
                _canClick = true;
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                _canClick = false;
            }
            else
            {
                _canClick = true;
            }
        }
    }

    IEnumerator StickSpill()
    {        
            // step 1: Hero kick this stick
            _heroController.StateKick();
            yield return new WaitForSeconds(0.2f);
            SoundManager._instance.OnPlayAudio(SoundType.kick);
             
             // step 2: Stick is spilled
            _currentStick.Spill();
             yield return new WaitForSeconds(0.222222f);
            SoundManager._instance.OnPlayAudio(SoundType.fall);
            yield return new WaitForSeconds(0.333333f);
            
            // step3: Check header of stick touche the perfect point for nextcolumn
             float PosXCurrentStick = _currentStick.GetPosXStickHorizontal();
            StartCoroutine(CheckStickOnGoodPoint(PosXCurrentStick));
            
            // step 4: Player move to Header of stick
            _heroController.disTanceWithColX = _nextCol.GetPosXleft();
            _heroController.SetTarget(_currentStick.GetHeightStick());
            BackGroundController._instance.MoveToLeft();
            _heroController.StateRun();
            _heroController.isMoveX = true;
            yield return new WaitForSeconds (_heroController.CaculerTimeWait()); // T = S/V  /// with distance = 3.3795 => V= 3 => t =1.2998
            _heroController.isMoveX = false;
            _heroController.StateIdle();
            
            // step 5: Check Player on column ?
            StartCoroutine(CheckPlayerOnColumn(PosXCurrentStick));
    }
    IEnumerator CheckStickOnGoodPoint(float PosXCurrentStick )
    {
        bool isStickOnGoodPoint = _nextCol.StickOnGoodPoint(PosXCurrentStick);

        if (isStickOnGoodPoint)
        {
            GamePlay._instance.UpdateScore(1);

            // Get PlusOneTxt From Object Pool
            GamePlay._instance.EnablePerfectTxt();
            Vector3 _currentPlushOneTxt = new Vector3(_nextCol._goodPoint.transform.position.x+0.1f, _nextCol._goodPoint.transform.position.y+0.1f, _nextCol._goodPoint.transform.position.z);
            GameObject newPlusOne=  ObjectPooler._instance.SpawnFromPool("PlusOneTxt", _currentPlushOneTxt,Quaternion.identity);
            newPlusOne.GetComponent<PlusOneTxt>().DimAplaColor();
            SoundManager._instance.OnPlayAudio(SoundType.perfect);
            yield return new WaitForSeconds(2f);

            // Add PlusOneTxt to Object Pool
            string tagColumn = "PlusOneTxt";
            GamePlay._instance.DisablePerfectTxt();
            ObjectPooler._instance.AddElement(tagColumn, newPlusOne);
        }
    }
   IEnumerator CheckPlayerOnColumn(float PosXCurrentStick)
    {
        _heroController.countClick = 0;

        bool isPlayerOnColumn = _nextCol.PlayerOnColumn(PosXCurrentStick);
        if ((!isPlayerOnColumn || _heroController.IsFlipY()) && _heroController.heroState == HeroState.living)
        {
             GameOver();
        }
        else
        {
            if (_heroController.heroState == HeroState.living)
            {

                SoundManager._instance.OnPlayAudio(SoundType.score);
                GamePlay._instance.UpdateScore(1);

                // Move To EndPoint of Column
                _heroController.StateRun();
                _heroController.MoveToPoint(_nextCol._endPoint.transform.position);
                yield return new WaitForSeconds(0.5f);
                _nextCol.EnableGoodPoint();
                _heroController.StateIdle();
                ChangeColumns();

                BackGroundController._instance.FllowPlayer();   
                CameraController._instance.FllowToPlayer();

                yield return new WaitForSeconds(0.1f);

                Vector3 RealPosNextCol = new Vector3(_nextColumn.transform.position.x - 3, _nextColumn.transform.position.y, 0);
                _nextCol.MoveToTarget(RealPosNextCol);

                yield return new WaitForSeconds(0.23f);

                BornNewStick(_nextColumn);

                _heroController.countClick = 0;
                BornNewMelonFromObjectPool();

                _countTimeTut++;
                if (_countTimeTut == 1)
                {
                    GamePlay._instance.DimTutorial();
                }
                BornBackGroundFromObjectPool();
           
                yield return new WaitForSeconds(0.1f);
            }
       
        }

    }    

    public void GameOver()
    {
        _currentStick.Rotate90Degereeto180Degree();
        _heroController.MoveDown();
        SoundManager._instance.OnPlayAudio(SoundType.dead);
        StartCoroutine(WaitShakeCamera());
    }
    IEnumerator WaitShakeCamera()
    {
        CameraController._instance.Shake();
        yield return new WaitForSeconds(0.8f);
        UiController._instance.EnableGameOverPanel();
    }
    void BornBackGroundFromObjectPool()
    {
        Vector3 PosLastChild = BackGroundController._instance.GetPosXLastChildBGDynamic();

        if (Mathf.Abs(_heroController.transform.position.x - PosLastChild.x) < 9f)
        {
            BackGroundController._instance.BornNewBGDynamic();
            GameObject oldBackGround = BackGroundController._instance.OldBackGrounDynamic();
            string tagColumn = "bg_" + BackGroundController._instance.idBg;
            ObjectPooler._instance.AddElement(tagColumn, oldBackGround);
        }
    }
    public IEnumerator PlayerGoToEndPointOnCurrentCol()
    {
        yield return new WaitForSeconds(0.1f);
        _heroController.StateRun();
        _heroController.MoveToPoint(_currentCol._endPoint.gameObject.transform.position);
        yield return new WaitForSeconds(0.5f);
        _heroController.StateIdle();
    }
    public void LoadCurrentHero()
    {
        int idHero = DataPlayer.getInforPlayer().idHeroPlaying;
        GameObject newHero = Instantiate(Resources.Load("Hero/Object/Hero_" + idHero, typeof(GameObject))) as GameObject;
        UpLoadHero(newHero);
    }
    void ChangeColumns()
    {
         AddElementToObjectPool();
        isPlayerOnStick = false;
        _currentColumn = _nextColumn;
         _nextColumn = BornNewColumn();
         UpdateValueOfColums();
    }
    private void UpdateValueOfColums()
    {
        _currentCol = _currentColumn.GetComponent<Column>();
        _currentCol.EnableStick();
        _currentStick = _currentCol._stick.GetComponent<Stick>();
        _nextCol = _nextColumn.GetComponent<Column>();
    }    
    void AddElementToObjectPool()
    {
        GameObject _currentStick = _allSticks.transform.GetChild(0).gameObject;
        _currentStick.transform.parent = _currentColumn.transform;
        _currentColumn.transform.parent = ObjectPooler._instance.gameObject.transform;
        string tagColumn = "Col" + _currentColumn.name;
        ObjectPooler._instance.AddElement(tagColumn, _currentColumn);
    }

    private GameObject BornNewMelonFromObjectPool()
    {
        float PosYMelon = (Random.RandomRange(1, 3) == 2) ? -1.928f : -2.19f;
        Vector3 PosNewMelon = new Vector3(Random.RandomRange(_currentCol.GetPosXRight()+0.25f, _nextCol.GetPosXleft()-0.25f), PosYMelon, 0);
        GameObject newObMelon = ObjectPooler._instance.SpawnFromPool("Melon", PosNewMelon, Quaternion.identity);
        newObMelon.transform.parent = _allFuit.transform;
        ObjectPooler._instance.AddElement("Melon", newObMelon);
        return newObMelon;
    }

    private GameObject BornNewColumn()
    {
        Vector3 LastPosChild = _allColumns.transform.GetChild(_allColumns.transform.childCount - 1).position;
        Vector3 newPosChild = new Vector3(LastPosChild.x + Random.RandomRange(1.24f, 3.4f) +3f, LastPosChild.y, 0);

        int Column = Random.RandomRange(0, 5);

        GameObject newColumn = ObjectPooler._instance.SpawnFromPool("Col"+ Column, newPosChild, Quaternion.identity);
        newColumn.name = Column.ToString();
        newColumn.GetComponent<Column>().ResetColumn();
        return newColumn;
    }
    private void BornNewStick(GameObject newColumn)
    {

        GameObject _newStick = newColumn.GetComponent<Column>()._stick;

        newColumn.transform.parent = _allColumns.transform;

        _newStick.transform.parent = _allSticks.transform;

        // Set Scale for Stick
        _newStick.transform.localScale = new Vector3(0.004274033f, 0, 0);

    }
    public void AddCurrentSore(int Sore)
    {
        _currentSore += Sore;
    }
    public int GetCurrentSore()
    {
        return _currentSore;
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
    public void EnableSoundStickGrow()
    {
        SoundManager._instance.OnPlayAudio(SoundType.stick_grow_loop);
        SoundManager._instance.audioFx.loop = true;
    }
    public bool IsPlayerOnNextCol(float PosXPlayer)
    {
        return _nextCol.PlayerOnColumn(PosXPlayer);
    }
}
