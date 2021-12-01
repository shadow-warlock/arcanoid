using UnityEngine;
using UnityEngine.UI;

public class MenuController  : MonoBehaviour
{

    public Text coinsIndicator;
    private void Start()
    {
        coinsIndicator.text = CoinsStore.GetInstance().GetCoinsCount().ToString();
    }
}
