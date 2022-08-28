using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementShopHero : MonoBehaviour
{
    public Button _buyHeroBtn;
    public Image _heroImg;
    public int _idHero;

    public Text _pirceTxt;
    public GameObject _melonImg;
    public GameObject _backGroundImg;
    public bool _isBought;
     

    private void Awake()
    {
        _buyHeroBtn.onClick.AddListener(SelectHero);
    }
    public void LoadData(Image HeroImg, int IdHero, int Price,bool IsBought)
    {

    }
    public void IsBought()
    {
        _pirceTxt.gameObject.SetActive(false);
        _melonImg.gameObject.SetActive(false);
        _backGroundImg.gameObject.SetActive(false);
        _isBought = true;
    }
    void SelectHero()
    {
        if(!_isBought)
        {
            // buy
        }
        else
        {
            // go to Game home
        }
    }
   

}
