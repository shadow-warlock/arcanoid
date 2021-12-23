using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public abstract class Unit : MonoBehaviour
    {
        public Action OnCreate;
        public Action OnDelete; 
        public Action<Unit, int, bool> OnDamage;
        public Action  OnUpdate; 
        public Func<Unit, bool>  BeforeDie; 
        public Action<IStatus>  OnAddStatus;
        
        public int Hp { get; protected set; }
        public UnitData.UnitData data;
        public virtual int MAXHp => (int) (data.MaxHp * GetMaxHpModificator());
        
        private Queue<AbilityData> _usedAbilities = new Queue<AbilityData>();


        private bool _busy;
        public bool IsDie { get; private set; }

        // Start is called before the first frame update
        protected void Start()
        {
            Hp = MAXHp;
            foreach (TimeAbilityData ability in data.Abilities)
            {
                StartCoroutine(TimeAbilityThread(ability));
            }
            StartCoroutine(CastThread());
            foreach (StatusData data in data.DefaultStatuses)
            {
                AddStatus(data, this, GetModificator(null));
            }

            OnCreate?.Invoke();

        }

        private IEnumerator FindTarget(AbilityData abilityData, Action<Unit> callback)
        {
            Unit castTarget;
            do
            {
                bool summonExist = GameObject.FindWithTag("Summon") != null;
                castTarget = abilityData.Target switch
                {
                    AbilityData.TargetType.Caster => this,
                    AbilityData.TargetType.Enemy => GameObject.FindWithTag(GetTargetType(summonExist))
                        ?.GetComponent<Unit>(),
                    _ => null
                };
                yield return new WaitForSeconds(0.05f);
            } while (castTarget == null || castTarget.IsDie);

            callback(castTarget);
        }

        private void Cast(AbilityData abilityData)
        {
            StartCoroutine(FindTarget(abilityData, (Unit target) => {
                StartCoroutine(abilityData.Cast(gameObject.GetComponent<Animator>(), this, target, () =>
                {
                    _busy = false;
                }));
            }));
        }

        // Use for status target (not caster)
        public ICountStatus AddStatus(StatusData data, Unit caster, float multiplier)
        {
            ICountStatus status = StatusFactory.CreateStatus(data, caster, this, multiplier);
            status.Work();
            OnAddStatus?.Invoke(status);
            return status;
        }
        

        public abstract bool CanCast(AbilityData abilityData);

        public abstract void PreCastDoing(AbilityData abilityData);

        public void UseAbility(AbilityData abilityData)
        {
            _usedAbilities.Enqueue(abilityData);
        }
        
        private IEnumerator TimeAbilityThread(TimeAbilityData abilityData)
        {
            while (true)
            {
                float cooldownProgress = 0;
                while (cooldownProgress < 1)
                {
                    yield return new WaitForSeconds(0.05f);
                    cooldownProgress += 0.05f / abilityData.Cooldown;
                }
                
                if (!_usedAbilities.Contains(abilityData))
                {
                    UseAbility(abilityData);
                }
            }
        }
        
        
        private IEnumerator CastThread()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                if (_usedAbilities.Count != 0 && !_busy)
                {
                    _busy = true;
                    Cast(_usedAbilities.Dequeue());
                }
            }
        }

        protected abstract IEnumerator Die();
        public abstract int GetLevel();
        public abstract String GetTargetType(bool summonExist);
        public abstract float GetModificator(ManaType? type);
        protected abstract float GetMaxHpModificator();
    
        public int TakeDamage(Unit damager, int damage)
        {
            if (IsDie)
            {
                return 0;
            }

            int realDamage = Math.Min(damage, Hp);
            Hp -= realDamage;
            OnDamage?.Invoke(damager, realDamage, false);
            if (Hp == 0)
            {
                IsDie = BeforeDie == null || BeforeDie(damager);
                if (IsDie && this != null)
                {
                    StartCoroutine(Die());
                }
            }

            return realDamage;
        }
    
        public int TakeHeal(Unit healer, int heal)
        {
            if (IsDie)
            {
                return 0;
            }
            int realHeal = Math.Min(MAXHp - Hp, heal);
            Hp += realHeal;
            OnDamage?.Invoke(healer, realHeal, true);
            return realHeal;
        }
        

        public void Delete()
        {
            OnDelete?.Invoke();
            transform.SetParent(null);
            Destroy(gameObject);
        }

    }
}
