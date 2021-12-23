using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "DamageEffect", order = 51)]
public class DamageEffect : Effect
{
    
    [SerializeField]
    private int power;
    

    public override void Do(Unit.Unit caster, Unit.Unit finalTarget, float multiplier)
    {
        finalTarget.TakeDamage(caster, (int) (power * Random.Range(0.9f, 1.1f) * multiplier));
    }
}
