using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ability;
using Status;
using UnityEngine;

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
        
        private Queue<AbilityData> _usedAbilities = new Queue<AbilityData>();
        public List<Status.Status> Statuses { get; } = new List<Status.Status>();

        [SerializeField]
        private List<StatusData> defaultStatuses = new List<StatusData>(); 
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
            foreach (StatusData data in defaultStatuses)
            {
                AddStatus(data, this, 1);
            }

            if (OnCreate != null) OnCreate();

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
        public void AddStatus(StatusData statusData, Unit caster, float coefficient)
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
                    cooldownProgress += 0.05f / (abilityData.Cooldown * GetStatusesPower(StatusData.StatusType.CastCooldown));
                }
                
                if (!_usedAbilities.Contains(abilityData))
                {
                    UseAbility(abilityData);
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
                    Cast(_usedAbilities.Dequeue());
                }
            }
        }

        protected abstract IEnumerator Die();
        public abstract int GetLevel();
        public abstract String GetTargetType(bool summonExist);
        public abstract float GetModificator(AbilityData abilityData);
        protected abstract float GetMaxHpModificator();
    
        public void TakeDamage(int damage)
        {
            if (IsDie)
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
                IsDie = true;
                StartCoroutine(Die());
            }
        }
    
        public void TakeHeal(int heal)
        {
            if (IsDie)
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
