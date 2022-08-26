using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    public GameObject _endPoint;
    public GameObject _goodPoint;
    public GameObject _stick;

    public void EnableGoodPoint()
    {
        _goodPoint.SetActive(false);
    }    
    public void EnableStick()
    {
        _stick.SetActive(true);
    }
    public void TurnOffStick()
    {
        _stick.SetActive(false);
    }    
    public bool PlayerOnColumn(float PosXPlayer)
    {
        float posXLeft = transform.position.x-(transform.localScale.x * 9.78f)/2;
        float posXRight= transform.position.x +(transform.localScale.x * 9.78f) / 2;
        return (posXLeft <= PosXPlayer && PosXPlayer <= posXRight) ? true : false;
    }
    public float GetPosXleft()
    {
        return (transform.position.x - (transform.localScale.x * 9.78f) / 2);
    }
    public float GetPosXRight()
    {
        return transform.position.x + (transform.localScale.x * 9.78f) / 2;
    }
    public bool StickOnGoodPoint(float PosXStick)
    {
        float posLeft = _goodPoint.transform.position.x - (_goodPoint.transform.lossyScale.x * 4.51f) / 2;
        float posRight = _goodPoint.transform.position.x + (_goodPoint.transform.lossyScale.x * 4.51f) / 2;
        return (posLeft <= PosXStick && PosXStick <= posRight) ? true : false;
    }    
    public void ResetColumn()
    {
        _goodPoint.SetActive(true);
        _stick.GetComponent<Stick>().ResetStick();
    }
}
