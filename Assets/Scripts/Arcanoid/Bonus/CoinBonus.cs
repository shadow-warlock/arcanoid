using UnityEngine;

public class CoinBonus : Bonus
{
    public override string GetStoreCode()
    {
        return "";
    }

    public override void OnPickup(Platform platform)
    {
        GameController.GetInstance().Wizard.GainCoins(1);
    }
    
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out Platform platform))
        {
            OnPickup(platform);
            Destroy(gameObject);
        }
        if (collider.gameObject.TryGetComponent(out Floor floor))
        {
            Destroy(gameObject);
        }
    }
}
