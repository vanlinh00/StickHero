using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerG2 : MonoBehaviour
{
    [SerializeField] Button _restartBtn; 
    private void Awake()
    {
        _restartBtn.onClick.AddListener(RestartGame);   
    }
    void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

}
