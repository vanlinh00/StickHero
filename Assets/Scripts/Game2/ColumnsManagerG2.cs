using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnsManagerG2 : MonoBehaviour
{
    [SerializeField] GameObject _nextColumn;
   public void BornNewColumn()
    {
        int LastNumbCol = transform.childCount - 1;
        Vector3 PosChildColumn = transform.GetChild(LastNumbCol).transform.position;
        Vector3 NewPosCol = new Vector3(PosChildColumn.x + Random.RandomRange(2.12f, 4f), PosChildColumn.y, 0);
        GameObject NewCol = ObjectPooler._instance.SpawnFromPool("Column", NewPosCol, Quaternion.identity);
        NewCol.transform.parent = transform;

        ObjectPooler._instance.AddElement("Column", transform.GetChild(0).transform.gameObject);
        transform.GetChild(0).transform.gameObject.transform.parent = ObjectPooler._instance.transform;
    }
    public Vector3 GetPosHeadNextCol()
    {
        int LastNumbCol = transform.childCount - 1;
         _nextColumn = transform.GetChild(LastNumbCol).gameObject;
        float PosYHead = _nextColumn.transform.position.y+ (_nextColumn.transform.localScale.y * 18.9f) / 2;
        return new Vector3(_nextColumn.transform.position.x, PosYHead+ 0.20199999f- 0.04899999f, 0);
    }
    public GameObject GetNextColum()
    {
        int LastNumbCol = transform.childCount - 1;
        _nextColumn = transform.GetChild(LastNumbCol).gameObject;
        return _nextColumn;
    }
}
