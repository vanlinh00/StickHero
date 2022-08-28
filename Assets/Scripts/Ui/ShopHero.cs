using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHero : MonoBehaviour
{
    [SerializeField] Button _quiteBg;
    [SerializeField] GameObject _conTent;
    [SerializeField] Text _amountMelon;

    private int _toTalHero=8;
    private void Awake()
    {
        _quiteBg.onClick.AddListener(QuiteShop);
    }
    private void OnEnable()
    {
        _amountMelon.text = GameManager._instance.GetCountCurrentLemon().ToString();
        Debug.Log(DataPlayer.getInforPlayer().listIdHero[0]);
       // LoadShopeHero();
    }
    void QuiteShop()
    {
        this.gameObject.SetActive(false);
    }
    void LoadShopeHero()
    {
       for(int i=0;i<_toTalHero;i++)
        {
            GameObject newButtonBuyHero = Instantiate(Resources.Load("Hero/ShopeHero/BuyHeroBtn", typeof(GameObject))) as GameObject;
            newButtonBuyHero.transform.parent = _conTent.transform;
        }
    }
}
