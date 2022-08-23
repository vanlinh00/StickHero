using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject _currentColumn;
    [SerializeField] GameObject _nextColumn;

    [SerializeField] GameObject _allColumns;
    [SerializeField] GameObject _allSticks;

    Column _currentCol;
    Column _nextCol;

    Stick _currentStick;

    public bool _isPlaying;
    public bool _canClick = true;

    private int _currentSore;

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
        if(Input.GetMouseButtonDown(0))
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

        if (_canClick == false)
        {
            return;
        }

        if (_isPlaying)
        {
         
            if (Input.GetMouseButton(0))
            {
                _currentStick.Grow();
            }
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(StickSpill());
            }

        }
    }
    
    IEnumerator StickSpill()
    {

        _currentStick.Spill();
        yield return new WaitForSeconds(0.5555555f);

        float PosXCurrentStick = _currentStick.GetPosXStickHorizontal();

        StartCoroutine(CheckStickOnGoodPoint(PosXCurrentStick));

        HeroController._instance.MoveByX(_currentStick.GetHeightStick());
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(CheckPlayOnColum(PosXCurrentStick));

    }

    IEnumerator CheckStickOnGoodPoint(float PosXCurrentStick )
    {
        bool isStickOnGoodPoint = _nextCol.StickOnGoodPoint(PosXCurrentStick);

        if (isStickOnGoodPoint)
        {
            GamePlay._instance.SetEnablePerfectTxt();
            yield return new WaitForSeconds(0.4f);
            GamePlay._instance.SetDenablePerfectTxt();
        }
    }
   IEnumerator CheckPlayOnColum(float PosXCurrentStick)
    {
        bool _isPlayrOnColumn = _nextCol.PlayerOnColumn(PosXCurrentStick);

        if (!_isPlayrOnColumn)
        {
            _currentStick.Rotate90Degereeto180Degree();

            HeroController._instance.MoveDown();

            yield return new WaitForSeconds(0.2f);

            UiController._instance.EnableGameOverPanel();

        }
        else
        {
            _nextCol.EnableGoodPoint();

            HeroController._instance.MoveToPoint(_nextCol._endPoint.transform.position);
            yield return new WaitForSeconds(0.5f);

            BackGroundController._instance.FllowToPlayer();
            CameraController._instance.FllowToPlayer();

            yield return new WaitForSeconds(0.1f);

            ChangeColumns();

        }

    }    
    void ChangeColumns()
    {
         AddElementToObjectPool();

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
    private GameObject BornNewColumn()
    {
        Vector3 LastPosChild = _allColumns.transform.GetChild(_allColumns.transform.childCount - 1).position;
        Vector3 newPosChild = new Vector3(LastPosChild.x + Random.RandomRange(2.24f, 3.4f), LastPosChild.y, 0);

        int Column = Random.RandomRange(1, 6);
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

}
