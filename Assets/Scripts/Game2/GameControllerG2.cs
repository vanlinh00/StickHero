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
   
    private bool _isStickPill = false;

    private int _currentScore = 0;

    void Update()
    {
        if (!_isStickPill)
        {
            _stick.GrowUp();
            _stick.GetDown();
        }
        if (Input.GetMouseButtonDown(0)&& !_isStickPill)
        {
            _stick.UpdateRatation();
            _stick.CaculerStartWidthTrail();

            _isStickPill = true;
            _stick.isStickSPill = true;

           StartCoroutine(HeroSPill());
        }   
    }
    private void Start()
    {
        BackGroundController._instance._currentPosXHero = _hero.transform.position.x;
    }
    IEnumerator HeroSPill()
    {
        yield return new WaitForSeconds(0.05000059631417f);
        _hero.isHeroSpill = true;
        yield return new WaitForSeconds(0.20059631417f);
        _stick.isStickSPill = false;
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.30059631417f);
        _hero.isHeroSpill = false;
   
        _stick.ResetStick();
        _hero.ResetHero();

        if (_stick._isCollisionMelon)
        {
            Vector3 PosHeadCol = columnsManager.GetPosHeadNextCol();
            Vector3 NewPosHero = new Vector3(PosHeadCol.x+ 0.141f, PosHeadCol.y + 0.092534f, 0);

            _hero._isMove = true;
            _hero.StateRotate();
            _hero.UpdateMoveMent(NewPosHero, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _hero._isMove = false;


            _stick.gameObject.SetActive(true);
            _hero.stickClone.SetActive(false);
            _hero.StateIdle();

            CameraController._instance.FllowToPlayer();
            _currentScore++;
             BackGroundController._instance.FllowPlayer();

            yield return new WaitForSeconds(0.3f);
            columnsManager.BornNewColumn();
            yield return new WaitForSeconds(0.5f);

            float disStickAndNextCol = columnsManager.GetNextColum().transform.position.x - _hero.transform.position.x;
            Vector3 PosA = new Vector3(columnsManager.GetNextColum().transform.position.x, _hero.transform.position.y, 0);

            melonManger.BornNewMelon(disStickAndNextCol, disStickAndNextCol, PosA);

            _stick._isCollisionMelon = false;

            _isStickPill = false;
        }
        else
        {
            _hero.StateDance();
            yield return new WaitForSeconds(1f);
            _hero.StateIdle();

            _isStickPill = false;
        }

     
        BornBackGroundFromObjectPool();
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

}
