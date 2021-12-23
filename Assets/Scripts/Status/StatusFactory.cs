using System;

public static class StatusFactory
{
    public static ICountStatus CreateStatus(StatusData data, Unit.Unit caster, Unit.Unit target, float multiplier)
    {
        return data switch
        {
            TimeStatusData timeData => new TimeStatus<TimeStatusData>(caster, target, timeData, multiplier),
            BeforeDieStatusData beforeDieData => new BeforeDieStatus(caster, target, beforeDieData, multiplier),
            OnDamageStatusData onDamageStatusData => new OnDamageStatus(caster, target, onDamageStatusData, multiplier),
            OnEndStatusData onEndStatusData => new OnEndStatus(caster, target, onEndStatusData, multiplier),
            _ => throw new NotSupportedException("Status data not supported in this factory")
        };
    }
    
}
