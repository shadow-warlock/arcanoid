using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability", order = 51)] 
public class Ability : ScriptableObject
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
    private EffectType type;

    [SerializeField] 
    private int power;
    
    [SerializeField]
    private Sprite icon;
    
    [SerializeField] 
    private int manaCost;
    
    [SerializeField] 
    private Wizard.ManaType manaType;
    
    [SerializeField] 
    private TargetType target;
    
    [SerializeField] 
    private String trigger;
    
    [SerializeField] 
    private float damageTime;
    

    public EffectType Type => type;
    
    public int Power => power;
    //
    public Sprite Icon => icon;
    //
    public int ManaCost => manaCost;
    //
    public TargetType Target => target;
    
    public Wizard.ManaType ManaType => manaType;
    public String Trigger => trigger;
    
    public float DamageTime => damageTime;


}
