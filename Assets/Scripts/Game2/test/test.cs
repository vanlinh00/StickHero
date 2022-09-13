using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float speed;
    public float distance;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        int layermask = 1 << 8;

        layermask = ~layermask;  // -257

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.up, distance, layermask);

        if(hitLeft.collider!=null)
        {
            Debug.Log(hitLeft.collider.name);
        }
    }
}
