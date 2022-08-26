using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlusOneTxt : MonoBehaviour
{
    [SerializeField] TMP_Text _textMesh;
   public void DimAplaColor()
    {
        StartCoroutine(FadeColor(250));
    }
    // 250/1   = 0.5/ x
    IEnumerator FadeColor(byte FadeColor)
    {  
        while (0 < FadeColor)
        {
            FadeColor -=1;
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 0.004f, transform.position.z);
            transform.position = newPos;
            yield return new WaitForEndOfFrame();
            _textMesh.color = new Color32(0, 0, 0, FadeColor);

        }

    }
}
