using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHome : Singleton<GameHome>
{
    [SerializeField] Button _playBtn;
    [SerializeField] Button _shopeHeroBtn;
    [SerializeField] GameObject _shopHero;

    [SerializeField] Button _audioBtn;
    protected override void Awake()
    {
        base.Awake();
        _playBtn.onClick.AddListener(PlayGame);
        _shopeHeroBtn.onClick.AddListener(OpenShope);
        _audioBtn.onClick.AddListener(ClickAudioBtn);
    }
    void ClickAudioBtn()
    {
        _audioBtn.gameObject.transform.GetChild(0).gameObject.SetActive(!_audioBtn.gameObject.transform.GetChild(0).gameObject.activeSelf);
    }
    void OpenShope()
    {
        _shopHero.SetActive(true);
    }
    void PlayGame()
    {
        SoundManager._instance.OnPlayAudio(SoundType.kick);
        UiController._instance.EnableGamePlayPanel();

        CameraController._instance.SetCameraGHToGP();
        BackGroundController._instance.SetBackGroundGHToGP();
    }

}
