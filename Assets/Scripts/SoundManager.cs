using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    button = 0,
    dead = 1,
    score = 2,
    perfect=3,
    kick=4,

}
public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioFx;
    private void OnValidate()
    {
        if (audioFx == null)
        {
            audioFx = gameObject.AddComponent<AudioSource>();
        }
    }
    public void OnPlayAudio(SoundType soundType)
    {
        var audio = Resources.Load<AudioClip>($"Audio/Sound/{soundType.ToString()}");
        audioFx.clip = audio;
        audioFx.Play();
    }

}