using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private float _speedScale= 0.26f;
    private float _smoothRotation =2.5f;

    private int _countCollideStick = 0;
    private bool _isCollideStickRight = false;

    private bool _isSpill = false;
    private bool _isSpill180 = false;

    // use for object pooling
    [SerializeField] Vector3 _oldlocalScale;

    private void Start()
    {
         gameObject.SetActive(false);
    }
    public void Grow()
    {
        if (!_isCollideStickRight && !_isSpill)
        {
            Vector3 newScale = new Vector3(transform.localScale.x, transform.localScale.y + Time.deltaTime * _speedScale, 0);
            transform.localScale = newScale;
        }

    }
    public void Spill()
    {
        if (!_isCollideStickRight&& !_isSpill)
        {
            StartCoroutine(FadeRotation(0f, -90f));
            _isSpill = true;
        }

    }
   public float GetHeightStick()
    {
        return transform.lossyScale.y* 18.9f;
    }
    public float GetPosXStickHorizontal()
    {
        return (transform.lossyScale.y * 18.9f)+transform.position.x;
    }
    public void Rotate90Degereeto180Degree()
    {
        if (!_isSpill180)
        {
            StartCoroutine(FadeRotation(-90f, -180f));
                _isSpill180 = true;
        }  
    }


    IEnumerator FadeRotation(float currentDegree, float Degree)
    {
        float t = currentDegree;
        while (t >= Degree)
        {
            yield return new WaitForEndOfFrame();
            t -= _smoothRotation;
            Quaternion target = Quaternion.Euler(transform.rotation.x, transform.rotation.y, (t > Degree) ? t : Degree);
            transform.rotation = target;
        }
    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Column"))
        {
            _countCollideStick++;
            if (_countCollideStick == 2)
            {
                _isCollideStickRight = true;
            }
        }
    } 

   public void ResetStick()
    {
        _countCollideStick = 0;
        _isCollideStickRight = false;

        _isSpill = false;
        _isSpill180 = false;

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = _oldlocalScale;
    }    
}
