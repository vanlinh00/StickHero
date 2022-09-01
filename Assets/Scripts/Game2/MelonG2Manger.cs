using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelonG2Manger : MonoBehaviour
{
 public  void BornNewMelon(float AB, float HeightStick, Vector3 PosHeadNextCol)
    {
        GameObject NewMeLon = ObjectPooler._instance.SpawnFromPool("Melon", ObjectPooler._instance.transform.position, Quaternion.identity);
        MelonG2 Melon = NewMeLon.GetComponent<MelonG2>();
        NewMeLon.GetComponent<MelonG2>().d = 1.38f * 2f* Melon.GetLocaleScale();
        NewMeLon.transform.position = Melon.CaculerPosMelon(AB,HeightStick,PosHeadNextCol);
        ObjectPooler._instance.AddElement("Melon",NewMeLon);
    }
}
