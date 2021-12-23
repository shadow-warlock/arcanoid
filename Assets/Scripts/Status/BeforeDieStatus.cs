using UnityEngine;

public class BeforeDieStatus : CountStatusDecorator<BeforeDieStatusData>
{

    public BeforeDieStatus(Unit.Unit caster, Unit.Unit target, BeforeDieStatusData data, float multiplier) : base(caster, target, data, multiplier)
    {
        target.BeforeDie += BeforeDie;
    }

    public override void Work()
    {
    }

    private bool BeforeDie(Unit.Unit killer)
    {
        foreach (Effect effect in Data.BeforeDieEffects)
        {
            effect.Do(Caster, Target, Multiplier);
        }
        
        foreach (Effect effect in Data.BeforeDieKillerEffects)
        {
            effect.Do(Caster, killer, Multiplier);
        }
        IncreaseCount(Data.IncreaseCount);
        Decorated?.IncreaseCount(Data.IncreaseDecoratedCount);
        
        return !Data.IsSave;
    }

    protected override void Delete()
    {
        OnDelete?.Invoke();
        Target.BeforeDie -= BeforeDie;
    }
}
