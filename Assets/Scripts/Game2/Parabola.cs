using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Parabola
{
    float heigh;
    public Parabola(float heigh)
    {
        this.heigh = heigh;
    }
    public void Move(Transform target, Vector3 a, Vector3 b, float time)
    {
        float target_X = a.x + (b.x - a.x) * time;
        float target_Y = a.y + ((b.y - a.y)) * time + heigh * (1 - (Mathf.Abs(0.5f - time) / 0.5f) * (Mathf.Abs(0.5f - time) / 0.5f));
        target.position = new Vector3(target_X, target_Y);
    }
}