using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelonG2 : MonoBehaviour
{
    public float d;
    [SerializeField] Animator _animator;
    public Vector3 oldPosition;
    private GameObject _effectBreak;
    public Rigidbody2D gigidbody;

    private void Start()
    {
        gigidbody = GetComponent<Rigidbody2D>();
    }
    // HeightStick = distance x stick to x nextcol
    public Vector3 CaculerPosMelon(float AB, float HeightStick, Vector3 PosHeadNextCol)
    {
        float BC = HeightStick+d/2;
        float AC = Mathf.Sqrt(BC * BC - AB * AB);     // AB=  distance x stick to x nextcol
        return new Vector3(PosHeadNextCol.x -d/2+ (Random.RandomRange(d/4,d/2)-0.2f),PosHeadNextCol.y+Random.RandomRange(-AC,AC), 0);
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
        _animator.SetBool("In", false);
    }
    public void Zoom()
    {
        _animator.SetBool("Break", false);
        _animator.SetBool("In", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stick"))
        {
            MelonG2Manger._instance.SetCountTouchMelon();
            AudioManager._instance.OnPlayAudio(SoundType.slice_watermelon_small);
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            _effectBreak = ObjectPooler._instance.SpawnFromPool("EffectBreak", transform.position, Quaternion.identity);
            Break();
            StartCoroutine(TimeWait());
        }
    }
    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(0.2f);
        // MoveDown();
        gigidbody.gravityScale = 5;
        yield return new WaitForSeconds(0.5f);
        Idle();

        gigidbody.gravityScale = 0;
        gigidbody.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.1f);
        gigidbody.bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        _effectBreak.SetActive(false);

        ObjectPooler._instance.AddElement("EffectBreak", _effectBreak);
        // gameObject.SetActive(false);
    }
    public void MoveDown()
    {
        Vector3 Target = new Vector3(transform.position.x, transform.position.y - 10f, 0);
        transform.DOMove(Target, 0.8f);
    }
    public void EnableMelon()
    {
        gameObject.SetActive(true);

    }
    public void EnableMelonAgain()
    {
        transform.position = oldPosition;
        Zoom();
       // gameObject.SetActive(true);
    }
    public void DisableMelon()
    {
        gameObject.SetActive(false);
    }
}
