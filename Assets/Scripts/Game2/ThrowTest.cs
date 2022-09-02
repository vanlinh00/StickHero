using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTest : MonoBehaviour
{
    public Transform a, b;
    public float duration;

    Parabola p;
    Vector3 startPos;
    float speedX, speedY;
    float preTime;
    void Start()
    {
        preTime = Time.time;
        startPos = a.position;
        p = new Parabola(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (((Time.time - preTime) / duration) <= 1)
            p.Move(a, startPos, b.position, (Time.time - preTime) / duration);
    }
}