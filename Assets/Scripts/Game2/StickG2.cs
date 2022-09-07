using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickG2 : MonoBehaviour
{
    [SerializeField] float _speedScale;
    [SerializeField] float _smoothRotation;

    private float _maxScale = 0.25f;      //  0.008999998  =>0.17009997732
    private float _minScale = 0.009f;   //                   3.5

    private bool _isScaleMax = false;
    private bool _isScaleMin = false;

    Vector3 newScale = new Vector3();

    [SerializeField] TrailRenderer trailRenderer;

    [SerializeField] float currentDegree;

    [SerializeField] float degree;

    [SerializeField] Vector3 _oldlocalScale;

    float timeCount = 0.0f;

    public bool isStickSPill = false;

    public bool _isCollisionMelon = false;

    public Transform a;
    private void Start()
    {
        _isScaleMax = true;
        _isScaleMin = false;
        trailRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isStickSPill)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, a.rotation, timeCount);
            timeCount = timeCount + Time.deltaTime;
        }

    }
    public void UpdateRatation()
    {
        timeCount = 0.0f;
    }

    public void GrowUp()
    {
        if (transform.localScale.y <= _maxScale && _isScaleMax)
        {
            newScale = new Vector3(transform.localScale.x, transform.localScale.y + Time.deltaTime * _speedScale, 0);
        }
        else
        {
            _isScaleMax = false;
            _isScaleMin = true;
        }
        transform.localScale = newScale;
    }
    public void GetDown()
    {
        if (transform.localScale.y >= _minScale && _isScaleMin)
        {
            newScale = new Vector3(transform.localScale.x, transform.localScale.y - Time.deltaTime * _speedScale, 0);
        }
        else
        {
            _isScaleMax = true;
            _isScaleMin = false;
        }
        transform.localScale = newScale;
    }

    public void Spill()
    {
        CaculerStartWidthTrail();
        StartCoroutine(FadeRotation(currentDegree, degree));
    }
    //     150              -90           => 240  /19  * Time*deltaTime
    IEnumerator FadeRotation(float currentDegree, float degree)
    {
        float t = currentDegree;
        while (t >= degree)
        {
            yield return null;
            t -= _smoothRotation;
            Quaternion target = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (t > degree) ? t : degree);
            transform.rotation = target;
        }
    }
    // 0.5   0.02978008
    // x =?    y = ok     => x     
    public void CaculerStartWidthTrail()
    {
        trailRenderer.startWidth = (0.5f * transform.localScale.y) / 0.02978008f -0.1f;
        trailRenderer.gameObject.SetActive(true);
    }
    public void ResetStick()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = _oldlocalScale;
        trailRenderer.gameObject.SetActive(false);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Melon"))
        {
            _isCollisionMelon = true;
        }

    }
}
