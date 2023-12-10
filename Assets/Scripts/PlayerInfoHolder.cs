using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfoHolder
{
    public const int REWARD = 550;
    private const string COINS_KEY = "Coins";
    private const string PASSED_LEVELS = "PassedLevels";

    public static event Action CoinsAmtUpdated;
    public static Dictionary<Ability.Abilities, int> PriceList = new() {
        { Ability.Abilities.Finger, 240 },
        {Ability.Abilities.Hammer, 300 },
        {Ability.Abilities.Teleportation, 350 },
        {Ability.Abilities.ShootAgain, 400 }
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

    public static int PassedLevels
    {
        get
        {
            if (!PlayerPrefs.HasKey(PASSED_LEVELS))
                PlayerPrefs.SetInt(PASSED_LEVELS, 0);
            return PlayerPrefs.GetInt(PASSED_LEVELS);
        }
        set
        {
            PlayerPrefs.SetInt(PASSED_LEVELS + 1, value);
        }
    }

    public static void PassedLevel()
    {
        Debug.Log("Added passed level");
        //PassedLevels++;
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
        if (Coins <= PriceList[abilityType])
            return false;
        PlayerPrefs.SetInt(abilityType.ToString(), AbilityAmount(abilityType) + 1);
        PlayerPrefs.SetInt(COINS_KEY, Coins -= PriceList[abilityType]);
        return true;
    }

    public static void AbilityUsed(Ability.Abilities abilityType)
    {
        if (AbilityAmount(abilityType) > 0)
            PlayerPrefs.SetInt(abilityType.ToString(), AbilityAmount(abilityType) - 1);
    }
}
