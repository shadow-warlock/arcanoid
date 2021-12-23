using UnityEngine;

public abstract class CountStatusDecorator<T> : CountStatus<T> where T : CountStatusDecoratorData
{

    protected readonly ICountStatus Decorated;
    
    public CountStatusDecorator(Unit.Unit caster, Unit.Unit target, T data, float multiplier) : base(caster, target, data, multiplier)
    {
        if (Data.DecoratedData != null)
        {
            Decorated = target.AddStatus(Data.DecoratedData, caster, multiplier);
        }
    }

    public override void Work()
    {
        Decorated?.Work();
    }
}
