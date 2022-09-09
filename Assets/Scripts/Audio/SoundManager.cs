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
    eating_fruit=5,
    stick_grow_loop=6,
    bg_sea=7,
    fall=8,
    roll_up_down =9,
    slice_nothing=10,
    slice_watermelon_small=11,
    stick_fallen=12,
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    public AudioSource audioFx;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
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
        audioFx.clip = audio;
        audioFx.Play();
    }
}