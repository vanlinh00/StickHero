using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamePlay : Singleton<GamePlay>
{
    [SerializeField] Text _perfectTxt;
    [SerializeField] Text _scoreTxt;
    [SerializeField] Text _countLemonTxt;
    [SerializeField] GameObject _tutorialOb;

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        _perfectTxt.gameObject.SetActive(false);
    }
    public void SetDenableTutorialTxt()
    {
        _tutorialOb.SetActive(false);
    }
    public void SetEnablePerfectTxt()
    {
        _perfectTxt.gameObject.SetActive(true);
    }
    public void SetDenablePerfectTxt()
    {
        _perfectTxt.gameObject.SetActive(false);
    }
   public void UpdateScore(int Score)
    {
        GameManager._instance.SetCurrentSore(Score);
        int currentSore = GameManager._instance.GetCurrentSore();
        _scoreTxt.text = currentSore.ToString();
    }
    public void UpdateAmountLemon(int AmountLemon)
    {
        GameManager._instance.SetCountCurrentLemon(AmountLemon);
        int CountCurrentLemo = GameManager._instance.GetCountCurrentLemon();
        _countLemonTxt.text = CountCurrentLemo.ToString();
    }

}
