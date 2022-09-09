using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverG2 : Singleton<GameOverG2>
{
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
    public void RestartGame()
    {
        SoundManager._instance.OnPlayAudio(SoundType.kick);
        SceneManager.LoadScene(1);
    }
    public void GoToHome()
    {
        SoundManager._instance.OnPlayAudio(SoundType.kick);
        SceneManager.LoadScene(0);
    }
    public void EnableGameOver()
    {
        gameObject.SetActive(true);
    }
    public void SetCurrentScore()
    {
       _currentScoreTxt.text = GameControllerG2._instance.GetCurrentScore().ToString();
        SetBestScore(GameControllerG2._instance.GetCurrentScore());
    }
    public void SetBestScore(int CurrentScore)
    {
        int BestScore = DataPlayer.getInforPlayer().bestScoreG2;

        if (BestScore < CurrentScore)
        {
            BestScore = CurrentScore;

            DataPlayer.UpdateBestScoreG2(BestScore);
        }
        _bestScoreTxt.text = BestScore.ToString();
    }
}
