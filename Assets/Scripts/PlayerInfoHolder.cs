using System;
using UnityEngine;

public static class PlayerInfoHolder
{
    public static event Action CoinsAmtUpdated;

    private const string COINS_KEY = "Coins";

    public static int Coins
    {
        get
        {
            if (!PlayerPrefs.HasKey(COINS_KEY))
                PlayerPrefs.SetInt(COINS_KEY, 0);
            return PlayerPrefs.GetInt(COINS_KEY);
        }

        private set
        {
            PlayerPrefs.SetInt(COINS_KEY, value);
            CoinsAmtUpdated?.Invoke();
        }
    }

    public static void AddCoins(int amt)
    {
        if (amt < 0) return;
        Coins += amt;
    }

    public static bool WithdrawCoins(int amt)
    {
        if (amt < 0) return false;
        if (amt > Coins) return false;

        Coins -= amt;
        return true;
    }
}
