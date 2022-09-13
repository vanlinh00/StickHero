using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMelon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            AudioManager._instance.OnPlayAudio(SoundType.eating_fruit);
            GamePlayG2._instance.UpdateAmountMeLon(1);
        }
    }
}
