using System;
using System.Collections;
using Unit;
using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(fileName = "New Mana Ability", menuName = "Mana Ability", order = 51)] 
    public class ManaAbilityData : AbilityData
    {

        [SerializeField] 
        protected int manaCost;
    
        [SerializeField] 
        protected Wizard.ManaType manaType;

        [SerializeField]
        protected Sprite icon;
        
        public int ManaCost => manaCost;
        public Wizard.ManaType ManaType => manaType;
        public Sprite Icon => icon;

        protected override void ChangeType(Unit.Unit caster, Unit.Unit finalTarget)
        {
            if (!(caster is Wizard))
            {
                throw new Exception("Заклинатель - не маг." + caster.ToString());
            }
            base.ChangeType(caster, finalTarget);
        } 
    }
}
