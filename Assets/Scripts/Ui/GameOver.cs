using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : Singleton<GameOver>
{
    // ALlButton in GameOver
    [SerializeField] Button _restartBt;
    [SerializeField] Button _homeBt;

    [SerializeField] Text _currentScoreTxt;
    [SerializeField] Text _bestScoreTxt;

    protected override void Awake()
    {
        base.Awake();
        _restartBt.onClick.AddListener(RestartGame);
        _homeBt.onClick.AddListener(GoToHome);
    }
    private void OnEnable()
    {
        UpdateCurrentScore();
    }
    public void RestartGame()
    {
        // SaveData
        DataPlayer.UpdataLoadGameAgain(true);
        SceneManager.LoadScene(0);
    }
    public void GoToHome()
    {
        SceneManager.LoadScene(0);
    }
    
    public void UpdateCurrentScore()
    {
        int CurrentScore = GameManager._instance.GetCurrentSore();

        _currentScoreTxt.text = CurrentScore.ToString();
        UpdateBestScore(CurrentScore);
    }
    public void UpdateBestScore(int CurrentScore)
    {
        int BestScore = DataPlayer.getInforPlayer().BestScore;
        if(BestScore<CurrentScore)
        {
            BestScore = CurrentScore;

            DataPlayer.UpdateBestScore(BestScore);
        }
        _bestScoreTxt.text = BestScore.ToString();
    }
  
}
