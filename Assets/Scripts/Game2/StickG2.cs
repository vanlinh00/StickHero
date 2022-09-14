using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickG2 : MonoBehaviour
{
    [SerializeField] float _speedScale;
    [SerializeField] float _smoothRotation;

    private float _maxScale = 0.19f;      //  0.008999998  =>0.17009997732
    private float _minScale = 0.009f;   //                   3.5
    private float _height = 18.9f;

    private bool _isScaleMax = false;
    private bool _isScaleMin = false;

    Vector3 newScale = new Vector3();

    [SerializeField] TrailRenderer trailRenderer;

    [SerializeField] Vector3 _oldlocalScale;

    private float _angleRotaion = 0f;

    float timeCount = 0.0f;

    //public float _speed = 1f;

    public bool isStickSPill = false;

    private Transform _target;

    [SerializeField] SpriteRenderer _spriteRenderer;

    // check touch col
    public bool isTouchCol;

    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        _target = new GameObject().transform;

        _isScaleMax = true;
        _isScaleMin = false;
        trailRenderer.gameObject.SetActive(false);

        isTouchCol = false;
    }
    private void Update()
    {
        //    if (isStickSPill)
        //    {
        //        transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, timeCount /** _speed*/);
        //        timeCount = timeCount + Time.deltaTime;
        //    }
    }
    public void UpdateRatation(float RotationZ)
    {
        _angleRotaion = RotationZ;
        // _speed = Mathf.Abs(165 / RotationZ);
        //timeCount = 0.0f;
        _target.eulerAngles = new Vector3(0, 0, RotationZ);
    }
    public void GrowUp()
    {
        if (transform.localScale.y <= _maxScale && _isScaleMax)
        {
            newScale = new Vector3(transform.localScale.x, transform.localScale.y + Time.deltaTime * _speedScale, 0);
            transform.localScale = newScale;
        }
        else
        {
            _isScaleMax = false;
            _isScaleMin = true;
        }
    }
    public void GetDown()
    {
        if (transform.localScale.y >= _minScale && _isScaleMin)
        {
            newScale = new Vector3(transform.localScale.x, transform.localScale.y - Time.deltaTime * _speedScale, 0);
            transform.localScale = newScale;
        }
        else
        {
            _isScaleMax = true;
            _isScaleMin = false;
        }
    }
    public void Spill()
    {
        AudioManager._instance.OnPlayAudio(SoundType.slice_nothing);
         trailRenderer.emitting = true;
        CaculerStartWidthTrail();
        //StartCoroutine(FadeRotation(Mathf.Abs(_angleRotaion * 0.26f / 155)));
        StartCoroutine(FadeRotation(0.26f));
    }

    // 150              -90           => 240  /19  * Time* deltaTime
    //transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, timeCount* _speed);
    //timeCount = timeCount + Time.deltaTime;
    //IEnumerator FadeRotation(float currentDegree, float degree)
    //{
    //    float t = currentDegree;
    //    while (t >= degree)
    //    {
    //        yield return new WaitForEndOfFrame();
    //        t -= _smoothRotation * Time.deltaTime;
    //        Quaternion target = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (t > degree) ? t : degree);
    //        transform.rotation = target;
    //    }
    //}
    IEnumerator FadeRotation(float TotalTime)
    {
        var passed = 0f;
        var init = transform.rotation;
        while (passed < TotalTime) 
        {
            yield return new WaitForEndOfFrame();
            passed += Time.deltaTime;
            var normalized = passed / TotalTime;
            transform.rotation = Quaternion.Lerp(init, _target.rotation, normalized);
        }
        yield return new WaitForEndOfFrame();
        trailRenderer.emitting = false;
    }


    // 0.5   0.02978008
    // x =?    y = ok     => x     
    public void CaculerStartWidthTrail()
    {
        trailRenderer.startWidth = (0.5f * transform.localScale.y) / 0.02978008f +0.2f;
        trailRenderer.gameObject.SetActive(true);
    }
    public void ResetStick()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = _oldlocalScale;
        trailRenderer.gameObject.SetActive(false);
    } 
    public void EnableStick()
    {
        Color newColor = _spriteRenderer.color;
        newColor.a = 1f;
        _spriteRenderer.color = newColor;
    }
    public void DisnableStick()
    {
        Color newColor = _spriteRenderer.color;
        newColor.a = 0f;
        _spriteRenderer.color = newColor;
    }
    public Vector3 I()
    {
        return new Vector3(transform.position.x, transform.position.y, 0);
    }
    public float R()
    {
        return transform.lossyScale.y * _height;
    }
    public float crossPointBetweenCircleAndLinear(float PosX)
    {
        // Have I (a,b)

        //=>  circleEquation
        // ( x-a)^2 + (y-b)^2 = R^2

        // x^2 - 2xa + a^2 + y^2 - 2yb + b^2 = R^2

        // y^2 -2xb + ( x^2 - 2xa+ a^2+ b^2-R^2)

        // A = 1
        // B = -2*b
        // => C =x^2 - 2xa+ a^2+ b^2-R^2

        // I (a,b)
        float a = transform.position.x;
        float b = transform.position.y;
        float x = PosX;

        float A = 1;
        float B = -2 * b;
        float C = (x * x) - (2 * x * a) + (a * a) + (b * b) - (R()*R());

        float PosY = QuadraticEquation2(A, B, C);

        return PosY; // crossPoint ( PosX, PosY)
    }
  
   public float QuadraticEquation2(float A, float B, float C)
    {
        float a = A;
        float b = B;
        float c = C;
    
        float delta = b * b - 4 * a * c;
        float x1=0;
        float x2=0;
  
        if (delta > 0)
        {
            x1 = (float)((-b + Mathf.Sqrt(delta)) / (2 * a));
            x2 = (float)((-b - Mathf.Sqrt(delta)) / (2 * a));
           // Console.Write("Phuong trinh co 2 nghiem la: x1 = {0} va x2 = {1}", x1, x2);
        }
        else if (delta == 0)
        {
            x1 = (-b / (2 * a));
           //Console.Write("Phong trinh co nghiem kep: x1 = x2 = {0}", x1);
        }
        else
        {
           // Console.Write("Phuong trinh vo nghiem!");
        }

        return x1;
    }
    public Vector3 FindVectorTowardsI(Vector3 desVector)
    {
        return new Vector3(transform.position.x - desVector.x, transform.position.y - desVector.y, 0);
    }
    public float FindAngleRotationWithCrossPoint(Vector3 DesVectorB)
    {
        float Angle = -155f;

        if (DesVectorB.y != 0)
        {
            return FindAngleRotaion(DesVectorB);
        }
        return Angle;
    }
    public float FindAngleRotaion(Vector3 DesVectorB)
    {
        float Angle = 0f;

        Vector3 desVectorA = new Vector3(transform.position.x, transform.position.y + R(), 0);

        Vector3 A = FindVectorTowardsI(desVectorA);

        Vector3 B = FindVectorTowardsI(DesVectorB);

        // cos( A;B) = x*x'+y*y'/ can(x^2+y^2)*can(x'^2+y'^2)

        float cosAB = (A.x * B.x + A.y * B.y) / (Mathf.Sqrt(A.x * A.x + A.y * A.y) * Mathf.Sqrt(B.x * B.x + B.y * B.y));

         Angle = - Mathf.Acos(cosAB)*45f / 0.7853981634f;

        return Angle;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.CompareTag("Column"))
        {
            isTouchCol = true;
        }
    }
}
