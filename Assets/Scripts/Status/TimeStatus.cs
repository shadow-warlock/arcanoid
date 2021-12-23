
using System.Collections;
using UnityEngine;

public class TimeStatus<T> : CountStatus<T> where T: TimeStatusData
{

    private Coroutine _process;

    public TimeStatus(Unit.Unit caster, Unit.Unit target, T data, float multiplier) : base(caster, target, data, multiplier)
    {
    }

    public override void Work()
    {
        _process = Target.StartCoroutine(CalculateTick());
    }

    private IEnumerator CalculateTick()
    {
        while (IsNotEnd())
        {
            yield return new WaitForSeconds(Data.TickTime);
            foreach (Effect effect in Data.OnTickEffects)
            {
                effect.Do(Caster, Target, Multiplier);
            }
            IncreaseCount(1);
        }
    }
    
    protected override void Delete()
    {
        if (_process != null)
        {
            Target.StopCoroutine(_process);
        }
        base.Delete();
    }
}
