using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New On Damage Status", menuName = "OnDamageStatus", order = 51)]
public class OnDamageStatusData : CountStatusDecoratorData
{
    [SerializeField] 
    private List<Effect> onDamageTargetEffects = new List<Effect>();
    
    [SerializeField] 
    private List<Effect> onDamageDamagerEffects = new List<Effect>();

    public List<Effect> ONDamageTargetEffects => onDamageTargetEffects;

    public List<Effect> OnDamageDamagerEffects => onDamageDamagerEffects;
}
