using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : Singleton<StartGame>
{
    [SerializeField] Button _playBt;
    protected override void Awake()
    {
        base.Awake();
        _playBt.onClick.AddListener(PlayGame);
        
    }

    void PlayGame()
    {
        UiController._instance.EnableGamePlayPanel();
        CameraController._instance.SetCameraGamePlay();
    }

}
