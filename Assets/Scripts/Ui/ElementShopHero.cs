using System;
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
    public void LoadData(Sprite HeroImg, int IdHero, int Price)
    {
        _heroImg.sprite = HeroImg;
        _idHero = IdHero;
        _pirceTxt.text = Price.ToString();
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
            int _currentAmountMelon = GameManager._instance.GetCountCurrentLemon();
            int _price = Int16.Parse(_pirceTxt.text.ToString());
            if(_currentAmountMelon>=_price)
            {
                GameManager._instance.SetCountCurrentLemon(-1);
                IsBought();
                DataPlayer.AddNewIdHero(_idHero);
            }
            else
            {
                Debug.Log("load Buy Lemon");
            }
        }
        else
        {
            if (_idHero <= 4)
            {
                Vector3 oldPosHero = GameManager._instance.hero.transform.position;
                GameObject OldHero = GameObject.FindGameObjectWithTag("Player");
                DestroyObject(OldHero);

                GameObject newHero = Instantiate(Resources.Load("Hero/Object/Hero_" + _idHero, typeof(GameObject)), oldPosHero, Quaternion.identity) as GameObject;
                GameManager._instance.UpLoadHero(newHero);

                DataPlayer.getInforPlayer().idHeroPlaying = _idHero;
                ShopHero._instance.QuiteShop();
            }
            else
            {
                Debug.Log("don't have this object in your resource");
            }
        }
    }
   

}
