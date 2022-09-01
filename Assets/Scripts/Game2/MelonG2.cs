using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelonG2 : MonoBehaviour
{
   public float d;
    [SerializeField] Animator _animator;

    // HeightStick = distance x stick to x nextcol
    public Vector3 CaculerPosMelon(float AB, float HeightStick, Vector3 PosHeadNextCol)
    {
        float BC = HeightStick + d;
        float AC = Mathf.Sqrt(BC * BC - AB * AB);     // AB=  distance x stick to x nextcol
        return new Vector3(PosHeadNextCol.x - (Random.RandomRange(d/4,d/2)), Random.RandomRange(-AC+(d/(1.3f)), PosHeadNextCol.y), 0);
    }
    public float GetLocaleScale()
    {
        return transform.localScale.y;   // y = x 
    }
    public void Break()
    {
        _animator.SetBool("Break", true);
    }
    public void Idle()
    {
        _animator.SetBool("Break", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stick"))
        {
            Break();
            StartCoroutine(TimeWait());
        }
    }
    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
