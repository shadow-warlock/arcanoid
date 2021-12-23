
using System;

public interface ICountStatus : IStatus
{
    public void IncreaseCount(int count);

    Action OnEnd
    {
        get;
        set;
    }
}
