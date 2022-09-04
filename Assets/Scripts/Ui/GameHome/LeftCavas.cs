using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeftCavas : MonoBehaviour
{
    [SerializeField] Button newGameBtn;
    void Start()
    {
        
    }
    private void Awake()
    {
        newGameBtn.onClick.AddListener(OpenGameCutFruit);
    }
    void OpenGameCutFruit()
    {
        SceneManager.LoadScene(1);
    }

}
