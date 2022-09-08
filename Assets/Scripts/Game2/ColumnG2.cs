using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnG2 : MonoBehaviour
{
    private float _width = 9.78f;
    private float _height = 18.9f;
    public float linearEquations()
    {
        float PosX = transform.position.x - (transform.lossyScale.x*_width/2);
        //float PosYMax = transform.position.y + (transform.lossyScale.y * _height/2);

        //Vector3 A = new Vector3(PosX, transform.position.y, 0);
        //Vector3 B = new Vector3(PosX, PosYMax,0);
        
        return PosX;
    }
    public float HeaderPosY()
    {
       return transform.position.y + (transform.lossyScale.y * _height / 2);
    }
    public float HeaderPosX()
    {
        return transform.position.x - (transform.lossyScale.x * _width / 2);
    }
}
