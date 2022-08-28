﻿using System.Collections;
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
                BestScore = 0,
            };
            SaveData();
        }
    }

    private static void SaveData()
    {
        var data = JsonUtility.ToJson(inforPlayer);
        PlayerPrefs.SetString(ALL_DATA, data);
    }

    public static void UpdataLoadGameAgain(bool _idLoadGameAgain)
    {
        inforPlayer = new InforPlayer
        {
            idLoadGameAgain = _idLoadGameAgain,
            BestScore = DataPlayer.getInforPlayer().BestScore,
        };
        SaveData();
    }

    public static void UpdateBestScore(int Score)
    {
        inforPlayer = new InforPlayer
        {
            idLoadGameAgain = DataPlayer.getInforPlayer().idLoadGameAgain,
            BestScore = Score,
        };
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
    public int BestScore;
}
