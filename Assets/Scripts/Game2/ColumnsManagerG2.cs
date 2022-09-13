using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnsManagerG2 : MonoBehaviour
{
    [SerializeField] GameObject _nextColumn;
   public void BornNewColumn(Vector3 PosHero)
    {
        int LastNumbCol = transform.childCount - 1;
        Vector3 PosChildColumn = transform.GetChild(LastNumbCol).transform.position;
        Vector3 NewPosCol = new Vector3(PosChildColumn.x + Random.RandomRange(2.12f, 3.5f)+4f, -6.585f + Random.RandomRange(-1.2f,0f), 0);
        GameObject NewCol = ObjectPooler._instance.SpawnFromPool("Column", NewPosCol, Quaternion.identity);
        _nextColumn = NewCol;
        NewCol.transform.parent = transform;
        ObjectPooler._instance.AddElement("Column", transform.GetChild(0).transform.gameObject);
        transform.GetChild(0).transform.gameObject.transform.parent = ObjectPooler._instance.transform;
        Vector3 NewPos = new Vector3(NewCol.transform.position.x-4f, NewPosCol.y , 0);
        StartCoroutine(WaitTimeNextColMove(NewPos, 0.2f, PosHero));
    }
   public void BornNewSmallMelon()
    {
        Vector3  PosHeadNextCol =  new Vector3( GetPosHeadNextCol().x,GetPosHeadNextCol().y+0.15f,0);
        GameObject SmallMelon = ObjectPooler._instance.SpawnFromPool("MelonSm", PosHeadNextCol, Quaternion.identity);
        ObjectPooler._instance.AddElement("MelonSm", SmallMelon);
    }

    IEnumerator WaitTimeNextColMove(Vector3 TargetPosCol, float TimeMove, Vector3 PosHero)
    {
        yield return new WaitForSeconds(0.2f);
        NextColMoveToTarget(TargetPosCol, TimeMove);
        BornNewMelon(TargetPosCol, PosHero);
        yield return new WaitForSeconds(0.2f);

        if (Random.RandomRange(1, 3) == 1)
        {
            BornNewSmallMelon();
        }
    }
    void BornNewMelon(Vector3 TargetPosCol,Vector3 PosHero)
    {
        float disStickAndNextCol = TargetPosCol.x - PosHero.x;
        Vector3 PosA = new Vector3(TargetPosCol.x, PosHero.y, 0);

        MelonG2Manger._instance.BornNewMelon(disStickAndNextCol, disStickAndNextCol, PosA);
        MelonG2Manger._instance.CurrentMelonMoveToTarget(MelonG2Manger._instance.GetCurrentMelon(), 0.2f);

        if (MelonG2Manger._instance.GetCurrentMelon2() != null)
        {
            MelonG2Manger._instance.CurrentMelonMoveToTarget(MelonG2Manger._instance.GetCurrentMelon2(), 0.2f);
        }
    }
    public void NextColMoveToTarget(Vector3 Target, float TimeMove)
    {
         StartCoroutine(Move(_nextColumn.transform, Target, TimeMove));
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
    public Vector3 GetPosHeadNextCol()
    {
        int LastNumbCol = transform.childCount - 1;
         _nextColumn = transform.GetChild(LastNumbCol).gameObject;
        float PosYHead = _nextColumn.transform.position.y+ (_nextColumn.transform.localScale.y * 18.9f) / 2;
        return new Vector3(_nextColumn.transform.position.x, PosYHead, 0);
    }
    public GameObject GetNextColum()
    {
        return _nextColumn;
    }

   
}
