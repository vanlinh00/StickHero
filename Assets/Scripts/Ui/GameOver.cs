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

    [SerializeField] CanvasGroup _canvasGroup;
    protected override void Awake()
    {
        base.Awake();
        _restartBt.onClick.AddListener(RestartGame);
        _homeBt.onClick.AddListener(GoToHome);
    }
    private void Start()
    {
        UpdateCurrentScore();
    }
    public void RestartGame()
    {
        SoundManager._instance.OnPlayAudio(SoundType.kick);

        // SaveData
        DataPlayer.UpdataLoadGameAgain(true);
        SceneManager.LoadScene(0);
    }
    public void GoToHome()
    {
        SoundManager._instance.OnPlayAudio(SoundType.kick);
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
        int BestScore = DataPlayer.getInforPlayer().bestScore;
        if(BestScore<CurrentScore)
        {
            BestScore = CurrentScore;

            DataPlayer.UpdateBestScore(BestScore);
        }
        _bestScoreTxt.text = BestScore.ToString();
    }
    public void In()
    {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        float t = 0;
        while(_canvasGroup.alpha<1)
        {
            yield return new WaitForEndOfFrame();
            _canvasGroup.alpha = t;
            t += Time.deltaTime*1.7f;
        }
    }
  
}
