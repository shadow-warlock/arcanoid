using System;
using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public enum TargetType
    {
        Caster,
        Enemy
    }
    
    [SerializeField] protected TargetType target;

    [SerializeField] protected string trigger;

    [SerializeField] private float actionTime;

    [SerializeField] private List<Effect> targetEffects = new List<Effect>();

    [SerializeField] private List<Effect> casterEffects = new List<Effect>();


    public List<Effect> TargetEffects => targetEffects;
    public List<Effect> CasterEffects => casterEffects;

    public TargetType Target => target;
    public String Trigger => trigger;
    public float ActionTime => actionTime;

    public virtual IEnumerator Cast(Animator animator, Unit.Unit caster, Unit.Unit finalTarget, Action callback)
    {
        if (caster.CanCast(this))
        {
            caster.PreCastDoing(this);
            if (Trigger != "")
            {
                animator.SetTrigger(Trigger);
            }
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * ActionTime);

            Do(caster, finalTarget);

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * (1 - ActionTime));
            callback();
        }
        callback();
    }

    protected virtual void Do(Unit.Unit caster, Unit.Unit finalTarget)
    {
        foreach (Effect effect in TargetEffects)
        {
            effect.Do(caster, finalTarget, caster.GetModificator(getType()));
        }
        foreach (Effect effect in CasterEffects)
        {
            effect.Do(caster, caster, caster.GetModificator(getType()));
        }
    }

    protected abstract ManaType? getType();

}
