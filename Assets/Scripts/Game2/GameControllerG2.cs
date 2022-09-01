using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameControllerG2 : MonoBehaviour
{
    [SerializeField] ColumnsManagerG2 columnsManager;
    [SerializeField] MelonG2Manger melonManger;
     
    [SerializeField] HeroG2 _hero;
    [SerializeField] StickG2 _stick;
   
    private bool _isPill = false;

    void Update()
    {
        //CheckCantClick();
        //if (_canClick == false)
        //{
        //    return;
        //}
        if (!_isPill)
        {
            _stick.GrowUp();
            _stick.GetDown();
        }
        if (Input.GetMouseButtonDown(0)&& !_isPill)
        {
            _isPill = true;
            _stick.Spill();
            StartCoroutine(HeroSPill());
        }   
    }
    IEnumerator HeroSPill()
    {
        // 240/19* Time*deltaTime  =  0.2f
        yield return new WaitForSeconds(0.08f);
        _hero.Spill();
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.2f);
        _stick.ResetStick();

        yield return new WaitForSeconds(0.2f);
        _hero.ResetHero();

        yield return new WaitForSeconds(0.2f);
        _stick.ResetStick();

        Vector3 newPosHero = columnsManager.GetPosHeadNextCol();
        yield return new WaitForSeconds(0.1f);

        _hero.MoveToTarget(newPosHero,1f);
        yield return new WaitForSeconds(1f);

        CameraController._instance.FllowToPlayer();

        yield return new WaitForSeconds(0.3f);

        columnsManager.BornNewColumn();

        yield return new WaitForSeconds(0.3f);

        float disStickAndNextCol = columnsManager.GetNextColum().transform.position.x - _stick.transform.position.x;

        Vector3 PosA = new Vector3(columnsManager.GetNextColum().transform.position.x, _stick.transform.position.y, 0);

        melonManger.BornNewMelon(disStickAndNextCol, disStickAndNextCol, PosA);

        _isPill = false;
    }

}
