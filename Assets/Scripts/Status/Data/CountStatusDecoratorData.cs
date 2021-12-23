using UnityEngine;

public abstract class CountStatusDecoratorData : CountStatusData
{
    [SerializeField] 
    private int increaseCount = 0;
    [SerializeField] 
    private int increaseDecoratedCount = 0;
    [SerializeField] 
    private CountStatusData decoratedData;

    public int IncreaseCount => increaseCount;

    public int IncreaseDecoratedCount => increaseDecoratedCount;

    public CountStatusData DecoratedData => decoratedData;
}
