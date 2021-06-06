using System;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public enum EffectType
    {
        Damage,
        Heal
    }
    
    public enum TargetType
    {
        Caster,
        Enemy
    }
    
    [SerializeField]
    protected EffectType type;

    [SerializeField] 
    protected int power;
    
    [SerializeField]
    protected Sprite icon;
    
    // [SerializeField] 
    // protected int manaCost;
    
    // [SerializeField] 
    // protected Wizard.ManaType manaType;
    
    [SerializeField] 
    protected TargetType target;
    
    [SerializeField] 
    protected string trigger;
    
    [SerializeField] 
    private float damageTime;
    

    public EffectType Type => type;
    
    public int Power => power;
    //
    public Sprite Icon => icon;
    //
    // public int ManaCost => manaCost;
    //
    public TargetType Target => target;
    
    // public Wizard.ManaType ManaType => manaType;
    public String Trigger => trigger;
    
    public float DamageTime => damageTime;


}
