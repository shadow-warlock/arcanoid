using System;
using UnityEngine;


public class OnEndStatus : CountStatusDecorator<OnEndStatusData>
{
    public OnEndStatus(Unit.Unit caster, Unit.Unit target, OnEndStatusData data, float multiplier) : base(caster, target, data, multiplier)
    {
    }
    

    public override void Work()
    {
        if (Decorated != null)
        {
            Decorated.Work();
            Decorated.OnEnd += OnEndAction;
        }
    }

    protected void OnEndAction()
    {
        foreach (Effect effect in Data.TargetEffects)
        {
            effect.Do(Caster, Target, Multiplier);
        }
        foreach (Effect effect in Data.CasterEffects)
        {
            effect.Do(Caster, Caster, Multiplier);
        }
    }
    
}
