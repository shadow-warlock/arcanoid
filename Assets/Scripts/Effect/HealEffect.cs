using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "HealEffect", order = 51)]
public class HealEffect : Effect
{
    
    [SerializeField]
    private int power;
    
    public override void Do(Unit.Unit caster, Unit.Unit finalTarget, float multiplier)
    {
       finalTarget.TakeHeal(caster, (int) (power * Random.Range(0.9f, 1.1f) * multiplier));
    }
}
