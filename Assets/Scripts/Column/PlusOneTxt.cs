using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlusOneTxt : MonoBehaviour
{
    [SerializeField] TMP_Text textMesh;
   public void DimAplaColor()
    {
        StartCoroutine(FadeColor(250));
    }
    IEnumerator FadeColor(byte FadeColor)
    {  
        while (0 < FadeColor)
        {
            FadeColor -=5;
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y +0.02f, transform.position.z);
            transform.position = newPos;
            yield return new WaitForEndOfFrame();
            textMesh.color = new Color32(0, 0, 0, FadeColor);

        }

    }
}
