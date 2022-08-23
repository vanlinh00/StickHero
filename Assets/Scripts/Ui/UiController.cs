using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : Singleton<UiController>
{
 
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] Button _restartBt;

    [SerializeField] GameObject _startGamePanel;
    [SerializeField] GameObject _gamePlayPanel;


    private void OnEnable()
    {
        // Get data form DataPlayer
        InforPlayer inforPlayer = DataPlayer.getInforPlayer();

        if (inforPlayer.idLoadGameAgain)
        {
            // Set Camera
            Vector3 newPositionCamera = new Vector3(0.49f, 0, -10);
            CameraController._instance.gameObject.transform.position = newPositionCamera;
            EnableGamePlayPanel();
        }
        else
        { 
            EnableGameStartPanel();
        }

    }
    protected override void Awake()
    {
        base.Awake();
        _restartBt.onClick.AddListener(RestartGame);
    }
    public void EnableGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
        _startGamePanel.SetActive(false);
        _gamePlayPanel.SetActive(false);

    }    
    public void EnableGameStartPanel()
    {
        GameManager._instance._isPlaying = false;

        _gameOverPanel.SetActive(false);
        _startGamePanel.SetActive(true);
        _gamePlayPanel.SetActive(false);
    }    
    public void EnableGamePlayPanel()
    {
        // SaveData
        DataPlayer.UpdataLoadGameAgain(false);

        GameManager._instance._isPlaying = true;

        _gameOverPanel.SetActive(false);
        _startGamePanel.SetActive(false);
        _gamePlayPanel.SetActive(true);
    }    
   public void RestartGame()
    {
        // SaveData
        DataPlayer.UpdataLoadGameAgain(true);
        SceneManager.LoadScene(0);
    }   
}
