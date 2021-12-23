using Unit;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public ManaType? Type { get; set; } = null;

    public abstract void Do(Unit.Unit caster, Unit.Unit finalTarget, float multiplier);

}
