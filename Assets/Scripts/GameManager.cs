using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject _currentColumn;
    [SerializeField] GameObject _nextColumn;

    Column _currentCol;
    Column _nextCol;

    Stick _currentStick;

    private void Start()
    {
        UpdateValueOfColums();
    }
    private void Update()
    {
        if(Input.GetMouseButton(0))
         {
            _currentStick.Grow();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(StickSpillAndPlayerMove());
        }   
    }
    IEnumerator StickSpillAndPlayerMove()
    {

        _currentStick.Spill();
        yield return new WaitForSeconds(0.45f);

        HeroController._instance.MoveByX(_currentStick.GetHeightStick());
        yield return new WaitForSeconds(1f);

        float PosXCurrentStick = _currentStick.GetPosXStickHorizontal();

        if (!_nextCol.PlayerOnColumn(PosXCurrentStick))
        {
            _currentStick.Rotate90Degereeto180Degree();

             HeroController._instance.MoveDown(); 
        }
        else
        {
            HeroController._instance.MoveToPoint(_nextCol._endPoint.transform.position);
            yield return new WaitForSeconds(1f);

            MovePlatForm._instance.SetPositionXCamera();
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
        _currentColumn.transform.parent = ObjectPooler._instance.gameObject.transform;
        ObjectPooler._instance.AddElement("Col5", _currentColumn);
    }    
    private GameObject BornNewColumn()
    {
        Vector3 LastPosChild = transform.GetChild(transform.childCount - 1).position;
        Vector3 newPosChild = new Vector3(LastPosChild.x + Random.RandomRange(2.24f, 3.2f), LastPosChild.y, 0);

        GameObject newColumn = ObjectPooler._instance.SpawnFromPool("Col5", newPosChild, Quaternion.identity);

        newColumn.GetComponent<Column>().ResetColumn();

        newColumn.transform.parent = this.transform;
        return newColumn;
    
    }


}
