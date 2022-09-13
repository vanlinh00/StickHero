using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayG2 : Singleton<GamePlayG2>
{
    [SerializeField] Text _scoreTxt;
    [SerializeField] Text _countLemonTxt;
    [SerializeField] Text _perfectTxt;

    Animator _animatorScoreTxt;

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        _animatorScoreTxt = _scoreTxt.GetComponent<Animator>();
        int CountCurrentLemo = GameControllerG2._instance.GetCountCurrentLemon();
        _countLemonTxt.text = CountCurrentLemo.ToString();
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
    public void EnablePerfectTxt()
    {
        _perfectTxt.gameObject.SetActive(true);
    }
    public void DisablePerfectTxt()
    {
        _perfectTxt.gameObject.SetActive(false);
    }

    public void UpdateAmountMeLon(int AmountLemon)
    {
        GameControllerG2._instance.SetCountCurrentLemon(AmountLemon);
        int CountCurrentLemo = GameControllerG2._instance.GetCountCurrentLemon();
        _countLemonTxt.text = CountCurrentLemo.ToString();
    }
}
