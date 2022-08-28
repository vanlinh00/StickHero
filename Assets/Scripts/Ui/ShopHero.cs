using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHero : MonoBehaviour
{
    [SerializeField] Button _quiteBg;
    [SerializeField] GameObject _conTent;
    [SerializeField] Text _amountMelon;

    public ElementShopHero[] _listElementShopHero;
    private int _toTalHero=8;
    private void Awake()
    {
        _quiteBg.onClick.AddListener(QuiteShop);
    }
    private void OnEnable()
    {
        _amountMelon.text = GameManager._instance.GetCountCurrentLemon().ToString();
        LoadShopeHero();
    }
    void QuiteShop()
    {
        this.gameObject.SetActive(false);
    }
    void LoadShopeHero()
    {
       for(int i=0;i<_listElementShopHero.Length;i++)
        {
            int PriceHero = Random.RandomRange(40, 80);
            var HeroImg = Resources.Load<Sprite>("Hero/Img/hero-"+i);

            if (DataPlayer.getInforPlayer().listIdHero.Contains(i))
            {
                _listElementShopHero[i].IsBought();
            }

            _listElementShopHero[i].LoadData(HeroImg, i, PriceHero);
        }
    }
    private void OnValidate()
    {
        if (_listElementShopHero == null || _listElementShopHero.Length == 0)
        {
            _listElementShopHero = GetComponentsInChildren<ElementShopHero>();
        }
    }
}
