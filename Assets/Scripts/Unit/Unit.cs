using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ability;
using Status;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Unit
{
    public abstract class Unit : MonoBehaviour
    {

        private int _hp;
        public UnitData.UnitData data;
        protected int MAXHp => (int) (data.MaxHp * GetMaxHpModificator());

        public GameObject damagePrefab;
        public GameObject statusPanel;
        public GameObject statusPrefab;
        private Queue<Ability.Ability> _usedAbilities = new Queue<Ability.Ability>();
        private List<Status.Status> _statuses = new List<Status.Status>();
        [SerializeField]
        private List<StatusData> defaultStatuses = new List<StatusData>(); 
        private bool _busy;
        private bool _die;

        // Start is called before the first frame update
        protected void Start()
        {
            _hp = MAXHp;
            foreach (TimeAbility ability in data.Abilities)
            {
                StartCoroutine(TimeAbilityThread(ability));
            }
            UpdateHpBar();
            StartCoroutine(CastThread());
            foreach (StatusData data in defaultStatuses)
            {
                AddStatus(data, this, 1);
            }
            statusPanel.SetActive(true);
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
                    AddStatus(statusData, castTarget, GetModificator(ability));
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

        private void AddStatus(StatusData statusData, Unit target, float coefficient)
        {
            Transform statusesContainer = statusPanel.transform.GetChild(2);
            GameObject statusObj = Instantiate(statusPrefab, statusesContainer.position, Quaternion.identity, statusesContainer);
            Status.Status status = statusObj.GetComponent<Status.Status>();
            status.Data = statusData;
            status.Target = target;
            status.Caster = this;
            status.Coefficient = coefficient;
            _statuses.Add(status);
            status.Coroutine = StartCoroutine(StatusThread(status));
            DrawDamageUI(statusData.Text, true);
        }

        protected abstract bool CanCast(Ability.Ability ability);

        protected abstract void PreCastDoing(Ability.Ability ability);

        public void UseAbility(Ability.Ability ability)
        {
                _usedAbilities.Enqueue(ability);
        }
    
        private IEnumerator StatusThread(Status.Status status)
        {
            while (!status.isEnd())
            {
                yield return new WaitForSeconds(status.TickSize);
                status.Tick();
            }
            status.Delete();
            _statuses.Remove(status);
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
            return _statuses.Where(status => status.Type == type).Aggregate<Status.Status, float>(1, (current, status) => current * status.Power);
        }
        
        protected bool IsStatusExist(StatusData.StatusType type)
        {
            return _statuses.Any(status => status.Type == type);
        }
        
        protected Status.Status GetStatus(StatusData.StatusType type)
        {
            return _statuses.Find(status => status.Type == type);
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
        protected abstract String GetTargetType(bool summonExist);
        protected abstract float GetModificator(Ability.Ability ability);
        protected abstract float GetMaxHpModificator();
    
        public void TakeDamage(int damage)
        {
            if (_die)
            {
                return;
            }
            DrawDamageUI(damage.ToString(), false);
            _hp = Math.Max(0, _hp - damage);
            UpdateHpBar();
            if (_hp == 0)
            {
                if (IsStatusExist(StatusData.StatusType.Revival))
                {
                    TakeHeal(MAXHp);
                    Status.Status status = GetStatus(StatusData.StatusType.Revival);
                    status.Tick();
                    if (status.isEnd())
                    {
                        _statuses.Remove(status);
                    }
                    return;
                }
                _die = true;
                statusPanel.SetActive(false);
                StartCoroutine(Die());
            }
        }
    
        public void TakeHeal(int heal)
        {
            if (_die)
            {
                return;
            }
            DrawDamageUI(heal.ToString(), true);
            _hp = Math.Min(MAXHp, _hp + heal);
            UpdateHpBar();
        }

        private void UpdateHpBar()
        {
            statusPanel.transform.GetChild(1).GetComponent<Slider>().value = (float)_hp / MAXHp;
        }
        
        private void DrawDamageUI(String text, bool positive)
        {
            Transform damageUIContainer = statusPanel.transform.GetChild(3);
            GameObject damageUI = Instantiate(damagePrefab, damageUIContainer.position, Quaternion.identity);
            damageUI.transform.SetParent(damageUIContainer);
            damageUI.transform.localScale = new Vector3(1, 1, 1);
            damageUI.GetComponent<Text>().text = text;
            if (positive)
            {
                damageUI.GetComponent<Text>().color = new Color(0.25f, 1f, 0.24f);
            }
        }
        
        public void Delete()
        {
            transform.SetParent(null);
            statusPanel.SetActive(false);
            foreach (Status.Status status in _statuses)
            {
                status.Delete();
            }
            _statuses.Clear();
            Destroy(gameObject);

            Transform damageUIContainer = statusPanel.transform.GetChild(3);
            for(int i=0; i < damageUIContainer.childCount; i++)
            {
                Destroy(damageUIContainer.GetChild(i).gameObject);
            }
        }

    }
}
