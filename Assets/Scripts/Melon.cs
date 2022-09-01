using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GamePlay._instance.UpdateAmountMeLon(1);
            AudioManager._instance.OnPlayAudio(SoundType.eating_fruit);
            gameObject.SetActive(false);
        }
    }

}