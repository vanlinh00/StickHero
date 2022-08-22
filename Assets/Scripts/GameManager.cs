using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject _currentColumn;
    public GameObject _nextColumn;

    Column _currentCol;
    Column _nextCol;

    Stick _currentStick;

    private void Start()
    {
        _currentCol = _currentColumn.GetComponent<Column>();
        _currentCol.SetActiveStick();
        _currentStick = _currentCol._stick.GetComponent<Stick>();


        _nextCol = _nextColumn.GetComponent<Column>();
    }
    private void Update()
    {
        if(Input.GetMouseButton(0))
         {
            _currentCol._stick.GetComponent<Stick>().Grow();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(StickSpillAndPlayerMove());
        }   
    }
    IEnumerator StickSpillAndPlayerMove()
    {
        _currentStick.Spill();
        yield return new WaitForSeconds(0.5f);

        HeroController._instance.MoveByX(_currentStick.GetHeightStick());
        yield return new WaitForSeconds(0.5f);

        if (!_nextCol.PlayerOnColumn(_currentStick.GetPosXStickHorizontal()))
        {
            _currentStick.Rotate90Degereeto180Degree();
             HeroController._instance.MoveDown(); 
        }
        else
        {
            HeroController._instance.MoveToPoint(_nextCol._endPoint.transform.position);
            yield return new WaitForSeconds(0.1f);

            MovePlatForm._instance.SetPositionXCamera();
            yield return new WaitForSeconds(0.1f);

            ChangeColumns();
        }    
    }
    void ChangeColumns()
    {
        _currentColumn = _nextColumn;
        _nextColumn = BornNewColumn();

        _currentCol = _currentColumn.GetComponent<Column>();
        _currentCol.SetActiveStick();

        _currentStick = _currentCol._stick.GetComponent<Stick>();

        _nextCol = _nextColumn.GetComponent<Column>();

    }
    private GameObject BornNewColumn()
    {
        Vector3 LastPosChild = transform.GetChild(transform.childCount - 1).position;
        Vector3 newPosChild = new Vector3(LastPosChild.x + Random.RandomRange(2.24f, 3.2f), LastPosChild.y, 0);
        int randomColumn = 5;/*Random.RandomRange(2, 7);*/
        GameObject newColumn = Instantiate(Resources.Load("Column/" + randomColumn, typeof(GameObject)), newPosChild, Quaternion.identity) as GameObject;
        newColumn.transform.parent = this.transform;
        return newColumn;
       // newColumn.GetComponent<Column>()._stick.transform.parent = _worldTransform;
    }


}
