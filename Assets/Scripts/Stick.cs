using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] float _speedScale;
    [SerializeField] float _smoothRotation;

    private int _countCollideStick = 0;
    private bool _isCollideStickRight = false;

    bool _isSpill = false;
    bool _isSpill180 = false;

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
        if (!_isCollideStickRight&& !_isSpill180)
        {
            StartCoroutine(FadeRotation(-90f, -180f));
                _isSpill180 = true;
        }  
    }
    IEnumerator FadeRotation(float currentDegree, float Degree)
    {
        yield return new WaitForSeconds(0.35f);
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

}
