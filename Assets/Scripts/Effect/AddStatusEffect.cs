using UnityEngine;

[CreateAssetMenu(fileName = "New Add Status Effect", menuName = "AddStatusEffect", order = 51)]
public class AddStatusEffect : Effect
{
    
    [SerializeField]
    private StatusData data;
    
    public override void Do(Unit.Unit caster, Unit.Unit finalTarget, float multiplier)
    {
       finalTarget.AddStatus(data, caster, multiplier);
    }
}
