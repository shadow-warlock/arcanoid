public class Cast
{
    private readonly int _cost;
    private readonly int _damage;

    public int Cost => _cost;

    public int Damage => _damage;

    public Cast(int cost, int damage)
    {
        _cost = cost;
        _damage = damage;
    }
}
