using UnityEngine;
using UnityEngine.UI;

public abstract class Bonus : MonoBehaviour
{

    [SerializeField]
    protected Sprite sprite;

    [SerializeField]
    protected float defaultChance;
    
    [SerializeField]
    protected float chanceByStoreLevel;

    public Sprite Sprite => sprite;

    public float Chance => defaultChance + GetStoreChance();

    public float ChanceByStoreLevel => chanceByStoreLevel;

    public abstract string GetStoreCode();
    public abstract void OnPickup(Platform platform);

    private void Start()
    {
        GetComponent<Image>().sprite = sprite;
    }

    private float GetStoreChance()
    {
        return 0;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Floor floor))
        {
            transform.SetParent(null);
            Destroy(this);
        }
    }
    
    
}
