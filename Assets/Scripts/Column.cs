using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    public GameObject _endPoint;
    [SerializeField] GameObject _goodPoint;
    public GameObject _stick;

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
        float posLeft = transform.position.x-(transform.localScale.x * 9.78f)/2;
        float posRight= transform.position.x +(transform.localScale.x * 9.78f) / 2;
        return (posLeft <= PosXPlayer && PosXPlayer <= posRight) ? true : false;
    }
    public void ResetColumn()
    {
        _stick.GetComponent<Stick>().ResetStick();
    }    
    
}
