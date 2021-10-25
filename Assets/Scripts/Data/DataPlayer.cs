using System.Collections.Generic;
using UnityEngine;

public static class DataPlayer
{
    private const string ALL_DATA = "all_data";
    private static AllData allData;

    static DataPlayer()
    {
        //Chuyen doi du lieu tu JSON sang kieu AllData
        allData = JsonUtility.FromJson<AllData>(PlayerPrefs.GetString(ALL_DATA));

        //Neu du lieu dau vao = null, tuc la user vao lan dau tien
        //Khoi tao du lieu mac dinh cho user
        if(allData == null)
        {
            var playerDefaultId = 0;
            allData = new AllData
            {
                playerList = new List<int> { playerDefaultId },
                coin = 100,
            };

            SaveData();
        }
    }

    private static void SaveData()
    {
        var data = JsonUtility.ToJson(allData);
        PlayerPrefs.SetString(ALL_DATA, data);
    }

    public static bool IsOwnedPlayerWithId(int id)
    {
        return allData.IsOwnedPlayerWithId(id);
    }

    public static void AddPlayer(int id)
    {
        if (IsOwnedPlayerWithId(id))
        {
            return;
        }

        allData.AddPlayer(id);

        SaveData();
    }

    public static int GetCoin()
    {
        return allData.GetCoin();
    }

    public static void AddCoin(int value)
    {
        allData.AddCoin(value);

        SaveData();
    }

    public static void SubCoin(int value)
    {
        allData.SubCoin(value);

        SaveData();
    }

    public static bool IsEnoughMoney(int cost)
    {
        return allData.IsEnoughMoney(cost);
    }
}

public class AllData
{
    public List<int> playerList;
    public int coin;

    public bool IsOwnedPlayerWithId(int id)
    {
        return playerList.Contains(id);
    }

    public void AddPlayer(int id)
    {
        if (IsOwnedPlayerWithId(id))
        {
            return;
        }

        playerList.Add(id);
    }

    public int GetCoin()
    {
        return coin;
    }

    public void AddCoin(int value)
    {
        coin += value;
    }

    public void SubCoin(int value)
    {
        coin -= value;
    }

    public bool IsEnoughMoney(int cost)
    {
        return coin >= cost;
    }
}