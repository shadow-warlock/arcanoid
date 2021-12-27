using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public enum ShopPages
    {
        Arcanoid = 0,
        Spells = 1,
        Hero = 2
    }

    private static readonly Dictionary<ShopPages, List<ShopStore.Product>> pages =
        new()
        {
            {ShopPages.Arcanoid, new List<ShopStore.Product> {ShopStore.Product.Balls}},
            {ShopPages.Spells, new List<ShopStore.Product> { }},
            {ShopPages.Hero, new List<ShopStore.Product> {ShopStore.Product.MaxHp}},
        };

    [SerializeField] private List<ProductData> products;

    [SerializeField] private GameObject leftPage;

    [SerializeField] private GameObject rightPage;

    [SerializeField] private ShopPanel productPrefab;

    private ShopPages _openPage = ShopPages.Arcanoid;
    private int _pageNumber = 1;

    private void Start()
    {
        FillBook();
    }

    public void ChangeOpenPage(int newPage)
    {
        _openPage = (ShopPages) newPage;
        FillBook();
    }

    private void FillBook()
    {
        foreach (Transform child in leftPage.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rightPage.transform) {
            Destroy(child.gameObject);
        }
        pages.TryGetValue(_openPage, out List<ShopStore.Product> productTypes);
        int i = 0;
        GameObject activePage = null;
        foreach (var data in productTypes.Select(type => products.Find((ProductData data) => data.Type == type)))
        {
            switch (i)
            {
                case < 4:
                    activePage = leftPage;
                    break;
                case < 8:
                    activePage = rightPage;
                    break;
                default:
                    return;
            }

            ShopPanel product = Instantiate(productPrefab, activePage.transform);
            product.data = data;
            
            i++;
        }

        while (i % 4 != 0 && activePage != null)
        {
            GameObject empty = new GameObject("empty " + i);
            empty.AddComponent<RectTransform>();
            empty.AddComponent<LayoutElement>();
            empty.transform.SetParent(activePage.transform);
            i++;
        }
    }
}