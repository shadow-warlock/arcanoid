using UnityEngine;

public class CoinsStore
{
    private static CoinsStore _instance;
    
    public const string Coins = "COINS";
    
    private CoinsStore()
    {
        if (!PlayerPrefs.HasKey(Coins))
        {
            PlayerPrefs.SetInt(Coins, 0);
        }
    }

    public int GetCoinsCount()
    {
        return PlayerPrefs.GetInt(Coins);
    }
    
    public int AddCoins(int count)
    {
        PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) + count);
        return PlayerPrefs.GetInt(Coins);
    }

    public static CoinsStore GetInstance()
    {
        return _instance ??= new CoinsStore();
    }
}
