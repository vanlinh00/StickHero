using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : Singleton<UiController>
{
    [SerializeField] Button _restartBt;

    private void Awake()
    {
        _restartBt.onClick.AddListener(RestartGame);
    }
   public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }   
    public void SetActiveRestartGame()
    {
        _restartBt.gameObject.SetActive(true);
    }    
}
