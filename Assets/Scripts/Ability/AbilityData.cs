using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ability
{
    public abstract class AbilityData : ScriptableObject
    {
        public enum EffectType
        {
            Damage,
            Heal,
            Nothing
        }

        public enum TargetType
        {
            Caster,
            Enemy
        }

        [SerializeField] protected EffectType type;

        [SerializeField] protected int power;

        [SerializeField] protected TargetType target;

        [SerializeField] protected string trigger;

        [SerializeField] private float damageTime;

        [SerializeField] private List<StatusData> targetStatuses = new List<StatusData>();

        [SerializeField] private List<StatusData> casterStatuses = new List<StatusData>();


        public EffectType Type => type;
        public List<StatusData> TargetStatuses => targetStatuses;
        public List<StatusData> CasterStatuses => casterStatuses;

        public int Power => power;
        public TargetType Target => target;
        public String Trigger => trigger;
        public float DamageTime => damageTime;

        public virtual IEnumerator Cast(Animator animator, Unit.Unit caster, Unit.Unit finalTarget, Action callback)
        {
            if (caster.CanCast(this))
            {
                caster.PreCastDoing(this);
                if (Trigger != "")
                {
                    animator.SetTrigger(Trigger);
                }
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * DamageTime);
                foreach (StatusData statusData in TargetStatuses)
                {
                    finalTarget.AddStatus(statusData, caster, caster.GetModificator(this));
                }
                ChangeType(caster, finalTarget);
                foreach (StatusData statusData in CasterStatuses)
                {
                    caster.AddStatus(statusData, caster, caster.GetModificator(this));
                }
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * (1 - DamageTime));
                callback();
            }
            callback();
        }

        protected virtual void ChangeType(Unit.Unit caster, Unit.Unit finalTarget)
        {
            switch (Type)
            {
                case EffectType.Heal:
                    finalTarget.TakeHeal((int) (Power * caster.GetModificator(this) * Random.Range(0.9f, 1.1f)));
                    break;
                case EffectType.Damage:
                    finalTarget.TakeDamage((int) (Power * caster.GetModificator(this) * Random.Range(0.9f, 1.1f)));
                    break;
                case EffectType.Nothing:
                    break;
                default:
                    throw new NotSupportedException("not supported");
            }
        } 
    }
}