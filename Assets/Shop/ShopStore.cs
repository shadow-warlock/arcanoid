using System;
using UnityEngine;

public class ShopStore
{
    private static ShopStore _instance;

    public enum Product
    {
        Balls,
        MaxHp,
        MaxMana1,
        MaxMana2,
        MaxMana3
    }
    
    
    private ShopStore()
    {
        foreach (string product in Enum.GetNames(typeof(Product)))
        {
            if (!PlayerPrefs.HasKey(product))
            {
                PlayerPrefs.SetInt(product, 0);
            }
        }
    }

    public int GetProductCount(Product product)
    {
        return PlayerPrefs.GetInt(product.ToString());
    }

    public int IncreaseProductCount(Product product)
    {
        PlayerPrefs.SetInt(product.ToString(), PlayerPrefs.GetInt(product.ToString()) + 1);
        return PlayerPrefs.GetInt(product.ToString());
    }

    public static ShopStore GetInstance()
    {
        return _instance ??= new ShopStore();
    }
}
