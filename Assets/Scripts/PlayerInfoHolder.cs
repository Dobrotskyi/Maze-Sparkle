using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfoHolder
{
    private const string COINS_KEY = "Coins";

    public static event Action CoinsAmtUpdated;
    public static Dictionary<Ability.Abilities, int> PriceList = new() {
        { Ability.Abilities.Finger, 240 },
        {Ability.Abilities.Hammer, 300 }
    };

    public static int GetAbilityPrice(Ability.Abilities ability) => PriceList[ability];

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

    public static int AbilityAmount(Ability.Abilities abilityType)
    {
        if (!PlayerPrefs.HasKey(abilityType.ToString()))
            PlayerPrefs.SetInt(abilityType.ToString(), 0);
        return PlayerPrefs.GetInt(abilityType.ToString());
    }

    public static bool TryPurchaseAbility(Ability.Abilities abilityType)
    {
        //If enough coins
        PlayerPrefs.SetInt(abilityType.ToString(), AbilityAmount(abilityType) + 1);
        return true;
    }

    public static void AbilityUsed(Ability.Abilities abilityType)
    {
        if (AbilityAmount(abilityType) > 0)
            PlayerPrefs.SetInt(abilityType.ToString(), AbilityAmount(abilityType) - 1);
    }
}
