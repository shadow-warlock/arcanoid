using Component.Coins;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel  : MonoBehaviour
{
    public Text levelIndicator;
    public Text cost;
    public Text name;
    public Text description;
    public Button increaseLevel;
    public Image image;
    public ProductData data;
    private void Start()
    {
        int level = ShopStore.GetInstance().GetProductCount(data.Type);
        name.text = data.Name;
        description.text = data.Description;
        image.sprite = data.Image;
        UpdateUI(level);
    }

    public void OnBuy()
    {
        int level = ShopStore.GetInstance().GetProductCount(data.Type);
        CoinsStore.GetInstance().RemoveCoins(data.CalculateCost(level));
        level = ShopStore.GetInstance().IncreaseProductCount(data.Type);
        UpdateUI(level);
    }

    private void UpdateUI(int level)
    {
        levelIndicator.text = level.ToString();
        cost.text = data.CalculateCost(level).ToString();
        increaseLevel.enabled = data.CalculateCost(level) <= CoinsStore.GetInstance().GetCoinsCount();
    }
}
