using UnityEngine;
using UnityEngine.UI;

namespace Component.Coins
{
    public class CoinsPanel  : MonoBehaviour
    {
        public Text coinsIndicator;
        private void Start()
        {
            CoinsStore.GetInstance().OnStoreChange += UpdateUI;
            coinsIndicator.text = CoinsStore.GetInstance().GetCoinsCount().ToString();
        }

        private void OnDestroy()
        {
            CoinsStore.GetInstance().OnStoreChange -= UpdateUI;
        }

        public void UpdateUI(int coins)
        {
            coinsIndicator.text = coins.ToString();
        }
    }
}