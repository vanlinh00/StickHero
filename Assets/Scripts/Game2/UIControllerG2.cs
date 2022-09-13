using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerG2 : Singleton<UIControllerG2>
{
    [SerializeField] GameObject _gameOver;
    protected override void Awake()
    {
        base.Awake();
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
