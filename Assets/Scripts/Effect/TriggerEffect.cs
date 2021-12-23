using UnityEngine;

[CreateAssetMenu(fileName = "New Trigger Effect", menuName = "TriggerEffect", order = 51)]
public class TriggerEffect : Effect
{
    
    [SerializeField]
    private string trigger;
    public override void Do(Unit.Unit caster, Unit.Unit finalTarget, float multiplier)
    {
        finalTarget.gameObject.GetComponent<Animator>().SetTrigger(trigger);
    }
}
