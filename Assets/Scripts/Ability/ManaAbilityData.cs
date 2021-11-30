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

    }
}
