
using System;

public abstract class CountStatus<T> : Status<T>, ICountStatus where T : CountStatusData
{
    protected int CurrentCount = 0;

    protected CountStatus(Unit.Unit caster, Unit.Unit target, T data, float multiplier) : base(caster, target, data, multiplier)
    {
    }

    public void IncreaseCount(int count)
    {
        CurrentCount += count;
        OnTick?.Invoke(CurrentCount / (float)Data.Count);
        if (CurrentCount >= Data.Count)
        {
            OnEnd?.Invoke();
            OnDelete?.Invoke();
            Delete();
        }
    }

    public Action OnEnd { get; set; }

    protected bool IsNotEnd()
    {
        return CurrentCount < Data.Count;
    }


}
