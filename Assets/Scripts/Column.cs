using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    public GameObject _endPoint;
    [SerializeField] GameObject _goodPoint;
    public GameObject _stick;

    public void SetActiveStick()
    {
        _stick.SetActive(true);
    }
    public bool PlayerOnColumn(float posXPlayer)
    {
        //float posLeft= transform.localScale.x* transform.gameObject.GetComponent<SpriteRenderer>().drawMode=
        float posLeft = transform.position.x-(transform.localScale.x * 9.78f)/2;
        float posRight= transform.position.x +(transform.localScale.x * 9.78f) / 2;
        return (posLeft <= posXPlayer && posXPlayer <= posRight) ? true : false;
    }
    

    /*   void Start()
        {
            Debug.Log(_stick.transform.localScale.y);
            Debug.Log(_stick.transform.lossyScale.y);
        }*/
  

}
