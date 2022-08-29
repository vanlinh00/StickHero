using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : Singleton<UiController>
{
    [SerializeField] GameObject _gamePlayPanel;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _gameHomePanel;
  
    private void OnEnable()
    {
        // Get data form DataPlayer
        InforPlayer inforPlayer = DataPlayer.getInforPlayer();

        if (inforPlayer.idLoadGameAgain)
        {
            CameraController._instance.SetCameraGPTOGP();

            GameManager._instance.LoadCurrentHero();
            EnableGamePlayPanel();
        }
        else
        {
            EnableGameHomePanel();
        }

    }
    protected override void Awake()
    {
        base.Awake();
    }
    public void EnableGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
        _gameHomePanel.SetActive(false);
        _gamePlayPanel.SetActive(false);
    }    
    public void EnableGameHomePanel()
    {
        SoundManager._instance.OnPlayAudio(SoundType.bg_sea);
        GameManager._instance.isPlaying = false;
         GameManager._instance.LoadCurrentHero();

        _gameOverPanel.SetActive(false);
        _gameHomePanel.SetActive(true);
        _gamePlayPanel.SetActive(false);
    }    
    public void EnableGamePlayPanel()
    {
        GameManager._instance.isPlaying = true;
        StartCoroutine(GameManager._instance.PlayerGoToEndPointOnCurrentCol());
        _gameOverPanel.SetActive(false);
        _gameHomePanel.SetActive(false);
        _gamePlayPanel.SetActive(true);
    }    

}
