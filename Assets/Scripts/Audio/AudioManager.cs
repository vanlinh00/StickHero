using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
  
    public AudioSource audioFx;
    protected override void Awake()
    {
        base.Awake();
    }
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
        //audioFx.clip = audio;
        //audioFx.Play();
        audioFx.PlayOneShot(audio);

    }
}
