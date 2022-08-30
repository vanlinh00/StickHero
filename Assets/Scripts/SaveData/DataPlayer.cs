using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer
{
    private const string ALL_DATA = "all_data";
    private static InforPlayer inforPlayer;
    static DataPlayer()
    {
        // tranform data from json to inforPlayer
        inforPlayer = JsonUtility.FromJson<InforPlayer>(PlayerPrefs.GetString(ALL_DATA));

        // if all == null  it mean the user logged in for the first time
        // now you need to initialize default value for user

        if (inforPlayer == null)
        {
            inforPlayer = new InforPlayer
            {
                idLoadGameAgain = false,
                bestScore = 0,
                amountMelon = 0,
                listIdHero = new List<int>() { 0},
                idHeroPlaying=0,
            };
            SaveData();
        }
    }
    private static void SaveData()
    {
        var data = JsonUtility.ToJson(inforPlayer);
        PlayerPrefs.SetString(ALL_DATA, data);
    }
    public static void UpdataLoadGameAgain(bool IsLoadGameAgain)
    {
        inforPlayer.idLoadGameAgain = IsLoadGameAgain;
        SaveData();
    }

    public static void UpdateBestScore(int Score)
    {
        //inforPlayer = new InforPlayer
        //{
        //    idLoadGameAgain = DataPlayer.getInforPlayer().idLoadGameAgain,
        //    bestScore = Score,
        //    amountMelon = DataPlayer.getInforPlayer().amountMelon,
        //    listIdHero = new List<int>() { 0, 1, 2, 3 },
        //};
        inforPlayer.bestScore = Score;
        SaveData();
    }

    public static void UpdateAmountHero(int AmountMelon)
    {
        inforPlayer.amountMelon = AmountMelon;
        SaveData();
    }
    public static void AddNewIdHero(int IdHero)
    {
        inforPlayer.listIdHero.Add(IdHero);
        SaveData();
    }

    public static void UpdateHeroPlaying(int IdHero)
    {
        inforPlayer.idHeroPlaying = IdHero;
        SaveData();
    }
    public static InforPlayer getInforPlayer()
    {
        return inforPlayer;
    }
 
}
public class InforPlayer
{
    public bool idLoadGameAgain;
    public int bestScore;
    public int amountMelon;
    public List<int> listIdHero;
    public int idHeroPlaying;
}
