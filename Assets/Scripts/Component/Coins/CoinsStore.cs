using System;
using UnityEngine;

namespace Component.Coins
{
    public class CoinsStore
    {
        public Action<int> OnStoreChange;
        private static CoinsStore _instance;

        private const string Coins = "COINS";
    
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
            int coins = PlayerPrefs.GetInt(Coins);
            SendNotify(coins);
            return coins;
        }
        
        public int RemoveCoins(int count)
        {
            int coins = PlayerPrefs.GetInt(Coins);
            if (coins < count) 
                throw new Exception("Not enough coins");
            PlayerPrefs.SetInt(Coins, coins - count);
            coins = PlayerPrefs.GetInt(Coins);
            SendNotify(coins);
            return coins;
        }

        private void SendNotify(int coins)
        {
            if (OnStoreChange != null)
            {
                OnStoreChange(coins);
            }
        }

        public static CoinsStore GetInstance()
        {
            return _instance ??= new CoinsStore();
        }
    }
}
