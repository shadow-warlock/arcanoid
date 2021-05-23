using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{

    public float cooldown = 5.0f;
    public GameObject hpBar;
    protected int _hp;

    public int MAXHp => (int) (_maxHp * GetMaxHpModificator());

    public GameObject damageUIContainer;
    public GameObject damagePrefab;
    private Coroutine _damageCoroutine;
    public Ability defaultAttack;
    private List<Ability> _usedAbilities = new List<Ability>();
    private bool _busy = false;
    private bool _die = false;
    private int _maxHp = 45;

    // Start is called before the first frame update
    protected void Start()
    {
        _hp = MAXHp;
        hpBar.GetComponent<Slider>().value = (float)_hp / MAXHp;
        _damageCoroutine = StartCoroutine(DamageThread());
    }
    
    private void Cast(Ability ability)
    {
        Unit enemy = GameObject.FindWithTag(GetTargetType())?.GetComponent<Unit>();
        Unit castTarget = null;
        switch (ability.Target)
        {
            case Ability.TargetType.Caster:
                castTarget = this;
                break;
            case Ability.TargetType.Enemy:
                castTarget = enemy;
                break;
        }

        if (castTarget != null && !castTarget._die)
        {
            Wizard wCaster = this as Wizard;
            if (wCaster == null || wCaster.HasMana(ability.ManaType, ability.ManaCost) )
            {
                if (wCaster != null)
                {
                    wCaster.RemoveMana(ability.ManaType, ability.ManaCost);
                }
                StartCoroutine(CastProcessing(gameObject.GetComponent<Animator>(), castTarget, ability));
                return;
            }
        }
        UseNextAbility();
    }

    private void UseNextAbility()
    {
        if (_usedAbilities.Count != 0 && !_busy)
        {
            Ability ability = _usedAbilities.ElementAt(0);
            _usedAbilities.RemoveAt(0);
            Cast(ability);
        }
    }
    
    private IEnumerator CastProcessing(Animator animator, Unit castTarget, Ability ability)
    {
        _busy = true;
        animator.SetTrigger(ability.Trigger);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * ability.DamageTime);
        switch (ability.Type)
        {
            case Ability.EffectType.Heal:
                castTarget.TakeHeal((int) (ability.Power * GetModificator(ability)));
                break;
            case Ability.EffectType.Damage:
                castTarget.TakeDamage((int) (ability.Power * GetModificator(ability)));
                break;
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * (1-ability.DamageTime));
        _busy = false;
        UseNextAbility();
    }
    
    protected void UseAbility(Ability ability)
    {
        if (!_busy)
        {
            Cast(ability);
        }
        else
        {
            _usedAbilities.Add(ability);
        }
    }
    
    
    private IEnumerator DamageThread()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            UseAbility(defaultAttack);
        }
    }

    protected abstract IEnumerator Die();
    protected abstract String GetTargetType();
    protected abstract float GetModificator(Ability ability);
    protected abstract float GetMaxHpModificator();
    
    public void TakeDamage(int damage)
    {
        if (_die)
        {
            return;
        }
        GameObject damageUI = Instantiate(damagePrefab, damageUIContainer.transform.position, Quaternion.identity);
        damageUI.transform.SetParent(damageUIContainer.transform);
        damageUI.transform.localScale = new Vector3(1, 1, 1);
        damageUI.GetComponent<Text>().text = damage.ToString();
        _hp = Math.Max(0, _hp - damage);
        hpBar.GetComponent<Slider>().value = (float)_hp / MAXHp;
        if (_hp == 0)
        {
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
        GameObject damageUI = Instantiate(damagePrefab, damageUIContainer.transform.position, Quaternion.identity);
        damageUI.transform.SetParent(damageUIContainer.transform);
        damageUI.transform.localScale = new Vector3(1, 1, 1);
        damageUI.GetComponent<Text>().text = heal.ToString();
        damageUI.GetComponent<Text>().color = new Color(0.25f, 1f, 0.24f);
        _hp = Math.Min(MAXHp, _hp + heal);
        hpBar.GetComponent<Slider>().value = (float)_hp / MAXHp;
    }


}
