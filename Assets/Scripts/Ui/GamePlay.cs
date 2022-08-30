using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamePlay : Singleton<GamePlay>
{
    [SerializeField] Text _perfectTxt;
    [SerializeField] Text _scoreTxt;
    [SerializeField] Text _countLemonTxt;
    [SerializeField] Text _tutorialTxt;

    Animator _animatorTutorialTxt;
    Animator _animatorScoreTxt;
    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        _animatorTutorialTxt = _tutorialTxt.GetComponent<Animator>();
        _animatorScoreTxt = _scoreTxt.GetComponent<Animator>();

        _perfectTxt.gameObject.SetActive(false);
        _animatorTutorialTxt.SetBool("dim", false);
        _countLemonTxt.text = GameManager._instance.GetCountCurrentLemon().ToString();
    }
    public void DimTutorial()
    {
        _animatorTutorialTxt.SetBool("dim", true);
    }

    public void EnablePerfectTxt()
    {
        _perfectTxt.gameObject.SetActive(true);
    }
    public void DisablePerfectTxt()
    {
        _perfectTxt.gameObject.SetActive(false);
    }
    public void UpdateScore(int Score)
    {
        GameManager._instance.AddCurrentSore(Score);
        int currentSore = GameManager._instance.GetCurrentSore();
        _scoreTxt.text = currentSore.ToString();

        StartCoroutine(FadeOffAni());
    }
    IEnumerator FadeOffAni()
    {
        _animatorScoreTxt.SetBool("AddScore", true);
        yield return new WaitForSeconds(0.45f);
        _animatorScoreTxt.SetBool("AddScore", false);
    }    
    public void UpdateAmountMeLon(int AmountLemon)
    {
        GameManager._instance.SetCountCurrentLemon(AmountLemon);
        int CountCurrentLemo = GameManager._instance.GetCountCurrentLemon();
        _countLemonTxt.text = CountCurrentLemo.ToString();
    }

}
