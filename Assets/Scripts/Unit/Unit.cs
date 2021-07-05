using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ability;
using Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit
{
    public abstract class Unit : MonoBehaviour
    {
        public Action OnCreate;
        public Action OnDelete; 
        public Action<string, bool> OnDamage;
        public Action  OnUpdate; 
        public Action<Status.Status>  OnAddStatus;
        
        public int Hp { get; private set; }
        public UnitData.UnitData data;
        public int MAXHp => (int) (data.MaxHp * GetMaxHpModificator());
        
        private Queue<Ability.Ability> _usedAbilities = new Queue<Ability.Ability>();
        public List<Status.Status> Statuses { get; } = new List<Status.Status>();

        [SerializeField]
        private List<StatusData> defaultStatuses = new List<StatusData>(); 
        private bool _busy;
        private bool _die;

        // Start is called before the first frame update
        protected void Start()
        {
            Hp = MAXHp;
            foreach (TimeAbility ability in data.Abilities)
            {
                StartCoroutine(TimeAbilityThread(ability));
            }
            StartCoroutine(CastThread());
            foreach (StatusData data in defaultStatuses)
            {
                AddStatus(data, this, 1);
            }

            if (OnCreate != null) OnCreate();

        }
    
        private IEnumerator Cast(Ability.Ability ability)
        {
            Unit castTarget;
            do
            {
                bool summonExist = GameObject.FindWithTag("Summon") != null;
                castTarget = ability.Target switch
                {
                    Ability.Ability.TargetType.Caster => this,
                    Ability.Ability.TargetType.Enemy => GameObject.FindWithTag(GetTargetType(summonExist))?.GetComponent<Unit>(),
                    _ => null
                };
                yield return new WaitForSeconds(0.05f);
            } while (castTarget == null || castTarget._die);

            if (CanCast(ability))
            {
                PreCastDoing(ability);
                Animator animator = gameObject.GetComponent<Animator>();
                animator.SetTrigger(ability.Trigger);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * ability.DamageTime);
                foreach (StatusData statusData in ability.TargetStatuses)
                {
                    castTarget.AddStatus(statusData, castTarget, GetModificator(ability));
                }
                foreach (StatusData statusData in ability.CasterStatuses)
                {
                    AddStatus(statusData, this, GetModificator(ability));
                }
                switch (ability.Type)
                {
                    case Ability.Ability.EffectType.Heal:
                        castTarget.TakeHeal((int) (ability.Power * GetModificator(ability) * Random.Range(0.9f, 1.1f)));
                        break;
                    case Ability.Ability.EffectType.Damage:
                        castTarget.TakeDamage((int) (ability.Power * GetModificator(ability) * Random.Range(0.9f, 1.1f)));
                        break;
                    case Ability.Ability.EffectType.Summon:
                        ((Wizard)this).Summon((SummonAbility)ability);
                        break;
                }
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * (1-ability.DamageTime));
            }
            _busy = false;
        }

        // Use for status target (not caster)
        private void AddStatus(StatusData statusData, Unit caster, float coefficient)
        {
            Status.Status status = new Status.Status(statusData, caster, this, coefficient);
            Statuses.Add(status);
            StartCoroutine(status.Work());
            if (OnAddStatus != null) OnAddStatus(status);
        }
        
        public void OnStatusEnd(Status.Status status)
        {
            Statuses.Remove(status);
        }

        protected abstract bool CanCast(Ability.Ability ability);

        protected abstract void PreCastDoing(Ability.Ability ability);

        public void UseAbility(Ability.Ability ability)
        {
            _usedAbilities.Enqueue(ability);
        }
        
        private IEnumerator TimeAbilityThread(TimeAbility ability)
        {
            while (true)
            {
                float cooldownProgress = 0;
                while (cooldownProgress < 1)
                {
                    yield return new WaitForSeconds(0.05f);
                    cooldownProgress += 0.05f / (ability.Cooldown * GetStatusesPower(StatusData.StatusType.CastCooldown));
                }
                
                if (!_usedAbilities.Contains(ability))
                {
                    UseAbility(ability);
                }
            }
        }

        protected float GetStatusesPower(StatusData.StatusType type)
        {
            return Statuses.Where(status => status.Type == type).Aggregate<Status.Status, float>(1, (current, status) => current * status.Power);
        }
        
        protected bool IsStatusExist(StatusData.StatusType type)
        {
            return Statuses.Any(status => status.Type == type);
        }
        
        protected Status.Status GetStatus(StatusData.StatusType type)
        {
            return Statuses.Find(status => status.Type == type);
        }
        
        private IEnumerator CastThread()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                if (_usedAbilities.Count != 0 && !_busy)
                {
                    _busy = true;
                    StartCoroutine(Cast(_usedAbilities.Dequeue()));
                }
            }
        }

        protected abstract IEnumerator Die();
        public abstract int GetLevel();
        protected abstract String GetTargetType(bool summonExist);
        protected abstract float GetModificator(Ability.Ability ability);
        protected abstract float GetMaxHpModificator();
    
        public void TakeDamage(int damage)
        {
            if (_die)
            {
                return;
            }

            int realDamage = Math.Min(damage, Hp);
            Hp -= realDamage;
            if (OnDamage != null) OnDamage(realDamage.ToString(), false);
            if (Hp == 0)
            {
                if (IsStatusExist(StatusData.StatusType.Revival))
                {
                    Status.Status status = GetStatus(StatusData.StatusType.Revival);
                    status.TickHandler();
                    return;
                }
                _die = true;
                StartCoroutine(Die());
            }
        }
    
        public void TakeHeal(int heal)
        {
            if (_die)
            {
                return;
            }
            int realHeal = Math.Min(MAXHp - Hp, heal);
            Hp += realHeal;
            if (OnDamage != null) OnDamage(realHeal.ToString(), true);
        }
        

        public void Delete()
        {
            if (OnDelete != null) OnDelete();
            foreach (Status.Status status in Statuses)
            {
                status.Stop();
            }
            Statuses.Clear();
            transform.SetParent(null);
            Destroy(gameObject);
        }

    }
}
