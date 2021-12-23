using UnityEngine;

[CreateAssetMenu(fileName = "New Delete Effect", menuName = "DeleteEffect", order = 51)]
public class DeleteEffect : Effect
{
    
    public override void Do(Unit.Unit caster, Unit.Unit finalTarget, float multiplier)
    {
        finalTarget.Delete();
    }
}
