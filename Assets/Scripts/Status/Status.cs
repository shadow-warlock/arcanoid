
using System;
using UnityEngine;

public abstract class Status<T> : IStatus where T : StatusData
{
    public Unit.Unit Caster { get; }
    public Unit.Unit Target { get; }
    public float Multiplier { get; }
    public T Data { get; }

    protected Status(Unit.Unit caster, Unit.Unit target, T data, float multiplier)
    {
        Caster = caster;
        Target = target;
        Data = data;
        Multiplier = multiplier;
        Target.OnDelete += Delete;
    }

    public abstract void Work();


    public Sprite Icon => Data.Icon;
    public bool IsView => Data.IsView;
    public Action OnDelete { get; set; }
    public Action<float> OnTick { get; set; }
    
    protected virtual void Delete()
    {
        OnDelete?.Invoke();
    }
}
