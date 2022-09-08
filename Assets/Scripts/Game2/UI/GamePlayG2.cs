using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayG2 : Singleton<GamePlayG2>
{
    [SerializeField] Text _scoreTxt;
    [SerializeField] Text _countLemonTxt;
   // [SerializeField] Text _tutorialTxt;

    Animator _animatorScoreTxt;

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        _animatorScoreTxt = _scoreTxt.GetComponent<Animator>();
    }
    public void UpdateScore(int Score)
    {
        GameControllerG2._instance.SetCurrentScore(Score);
        int currentScore = GameControllerG2._instance.GetCurrentScore();
        _scoreTxt.text = currentScore.ToString();
        StartCoroutine(FadeOffAni());
    }
    IEnumerator FadeOffAni()
    {
        _animatorScoreTxt.SetBool("AddScore", true);
        yield return new WaitForSeconds(0.45f);
        _animatorScoreTxt.SetBool("AddScore", false);
    }
    //public void UpdateAmountMeLon(int AmountLemon)
    //{
    //    GameManager._instance.SetCountCurrentLemon(AmountLemon);
    //    int CountCurrentLemo = GameManager._instance.GetCountCurrentLemon();
    //    _countLemonTxt.text = CountCurrentLemo.ToString();
    //}
}
