using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static HeroController;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject _currentColumn;
    [SerializeField] GameObject _nextColumn;

    [SerializeField] GameObject _allColumns;
    [SerializeField] GameObject _allSticks;
    [SerializeField] GameObject _allFuit;

    Column _currentCol;
    Column _nextCol;

    Stick _currentStick;

    public bool isPlaying;
    private bool _canClick = true;

    private int _currentSore;
    private int _countCurrentLemon;

    private bool _isPlayerOnStick = false;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _currentSore = 0;
        UpdateValueOfColums();
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
            if (Input.GetMouseButton(0))
            {
                _currentStick.Grow();
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!_isPlayerOnStick)
                {
                    _isPlayerOnStick = true;
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
            _currentStick.Spill();

            yield return new WaitForSeconds(0.5555555f);

            float PosXCurrentStick = _currentStick.GetPosXStickHorizontal();

            StartCoroutine(CheckStickOnGoodPoint(PosXCurrentStick));

            BackGroundController._instance.MoveToLeft();

            HeroController._instance.disTanceWithColX = _nextCol.GetPosXleft();

            HeroController._instance.SetTarget(_currentStick.GetHeightStick());

            //HeroController._instance.CaculerSpeed();

            HeroController._instance.isMoveX = true;

            // T = S/V      /// with distance = 3.3795 => V= 3     => t =1.1265

            yield return new WaitForSeconds (HeroController._instance.CaculerTimeWait());

            HeroController._instance.isMoveX = false;

        StartCoroutine(CheckPlayOnColum(PosXCurrentStick));
    }
    IEnumerator CheckStickOnGoodPoint(float PosXCurrentStick )
    {
        bool isStickOnGoodPoint = _nextCol.StickOnGoodPoint(PosXCurrentStick);

        if (isStickOnGoodPoint)
        {
            GamePlay._instance.SetEnablePerfectTxt();

            Vector3 _currentPlushOneTxt = new Vector3(_nextCol._goodPoint.transform.position.x+0.1f, _nextCol._goodPoint.transform.position.y+0.1f, _nextCol._goodPoint.transform.position.z);
           
            GameObject newPlusOne=  ObjectPooler._instance.SpawnFromPool("PlusOneTxt", _currentPlushOneTxt,Quaternion.identity);

            newPlusOne.GetComponent<PlusOneTxt>().DimAplaColor();

            SoundManager._instance.OnPlayAudio(SoundType.perfect);

            yield return new WaitForSeconds(2f);

            string tagColumn = "PlusOneTxt";

            ObjectPooler._instance.AddElement(tagColumn, newPlusOne);

            GamePlay._instance.SetDenablePerfectTxt();
        }
    }
   IEnumerator CheckPlayOnColum(float PosXCurrentStick)
    {
        HeroController._instance.countClick = 0;

        bool isPlayerOnColumn = _nextCol.PlayerOnColumn(PosXCurrentStick);

        if (!isPlayerOnColumn || HeroController._instance.IsFlipY() && HeroController._instance.heroState == HeroState.living)
        {
             GameOver();
        }
        else
        {
            if (HeroController._instance.heroState == HeroState.living)    
            {
                _nextCol.EnableGoodPoint();

                GamePlay._instance.UpdateScore(1);

                HeroController._instance.MoveToPoint(_nextCol._endPoint.transform.position);

                yield return new WaitForSeconds(0.5f);

                SoundManager._instance.OnPlayAudio(SoundType.score);

                ChangeColumns();

                MoveToPlayer._instance.FllowToPlayer();

                BornNewMelonFromObjectPool();

                yield return new WaitForSeconds(0.1f);

                BornBackGroundFromObjectPool();
            }
       
        }

    }    

    public void GameOver()
    {
        _currentStick.Rotate90Degereeto180Degree();
        HeroController._instance.MoveDown();
        SoundManager._instance.OnPlayAudio(SoundType.dead);
        StartCoroutine(FadeGameOver());
    }
    IEnumerator FadeGameOver()
    {
        yield return new WaitForSeconds(0.7f);
        UiController._instance.EnableGameOverPanel();
    }
    void BornBackGroundFromObjectPool()
    {      
        // born new backgoround and add to object pool
        BackGroundController._instance.BornNewBackGround();

        GameObject oldBackGround = BackGroundController._instance.gameObject.transform.GetChild(0).gameObject;

        string tagColumn = "Bg_1";

        ObjectPooler._instance.AddElement(tagColumn, oldBackGround);

    }
    public IEnumerator PlayerGoToEndPointOnCurrentCol()
    {
        yield return new WaitForSeconds(0.1f);
        HeroController._instance.MoveToPoint(_currentCol._endPoint.gameObject.transform.position);
    }
    void ChangeColumns()
    {
         AddElementToObjectPool();

        _isPlayerOnStick = false;

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

        Vector3 newPosChild = new Vector3(LastPosChild.x + Random.RandomRange(2.24f, 3.4f) , LastPosChild.y, 0);

        int Column = Random.RandomRange(0, 5);

        GameObject newColumn = ObjectPooler._instance.SpawnFromPool("Col"+ Column, newPosChild, Quaternion.identity);

        newColumn.name = Column.ToString();

        newColumn.GetComponent<Column>().ResetColumn();

        GameObject _newStick = newColumn.GetComponent<Column>()._stick;

        newColumn.transform.parent = _allColumns.transform;

        _newStick.transform.parent = _allSticks.transform;

        // Set Scale for Stick
        _newStick.transform.localScale = new Vector3(0.004274033f, 0, 0);

        return newColumn;
    }
    public void SetCurrentSore(int Sore)
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
    }
    public int GetCountCurrentLemon()
    {
        return _countCurrentLemon;
    }

}
