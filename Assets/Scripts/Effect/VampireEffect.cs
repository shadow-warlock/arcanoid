using UnityEngine;

[CreateAssetMenu(fileName = "New Vampire Effect", menuName = "VampireEffect", order = 51)]
public class VampireEffect : Effect
{
    
    [SerializeField]
    private int power;
    [SerializeField]
    private float vampirePercent;

    public override void Do(Unit.Unit caster, Unit.Unit finalTarget, float multiplier)
    {
        int damage = finalTarget.TakeDamage(caster, (int) (power * Random.Range(0.9f, 1.1f) * multiplier));
        caster.TakeHeal(caster, (int) (damage * vampirePercent));
    }
}
