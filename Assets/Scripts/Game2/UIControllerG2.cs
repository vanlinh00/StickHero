using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerG2 : Singleton<UIControllerG2>
{
    [SerializeField] GameObject _gameOver;
    [SerializeField] Button _restartbtn;
    protected override void Awake()
    {
        base.Awake();
        _restartbtn.onClick.AddListener(RestartGame);
    }
    void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void EnableGameOver()
    {
        _gameOver.SetActive(true);
    }
    public void DisableGameOver()
    {
        _gameOver.SetActive(false);
    }

}
