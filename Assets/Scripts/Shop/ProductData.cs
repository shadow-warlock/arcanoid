using UnityEngine;

[CreateAssetMenu(fileName = "New Product", menuName = "ProductData", order = 0)]
public class ProductData : ScriptableObject
{
    [SerializeField] 
    protected ShopStore.Product type;
    
    [SerializeField] 
    protected int cost;
    
    [SerializeField] 
    protected int costMultipleByLevel;

    [SerializeField] 
    protected Sprite image;
    
    [SerializeField] 
    protected string name;
    
    [SerializeField] 
    protected string description;

    public ShopStore.Product Type => type;

    public int Cost => cost;

    public int CostMultipleByLevel => costMultipleByLevel;

    public Sprite Image => image;

    public string Name => name;

    public string Description => description;

    public int CalculateCost(int level)
    {
        return (int) (Cost * (Mathf.Pow(level, CostMultipleByLevel) + 1));
    }
}
