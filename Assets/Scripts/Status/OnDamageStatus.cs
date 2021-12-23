using UnityEngine;


public class OnDamageStatus : CountStatusDecorator<OnDamageStatusData>
{
    public OnDamageStatus(Unit.Unit caster, Unit.Unit target, OnDamageStatusData data, float multiplier) : base(caster, target, data, multiplier)
    {
        target.OnDamage += OnDamage;
    }
    
    private void OnDamage(Unit.Unit damager, int damage, bool isPositive)
    {
        if (!isPositive)
        {
            foreach (Effect effect in Data.ONDamageTargetEffects)
            {
                effect.Do(Caster, Target, Multiplier);
            }
        
            foreach (Effect effect in Data.OnDamageDamagerEffects)
            {
                effect.Do(Caster, damager, Multiplier);
            }
            IncreaseCount(Data.IncreaseCount);
            Decorated?.IncreaseCount(Data.IncreaseDecoratedCount);
        }
    }

    public override void Work()
    {
    }
}
