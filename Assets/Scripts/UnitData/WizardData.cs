using System.Collections.Generic;
using Ability;
using UnityEngine;

namespace UnitData
{
    [CreateAssetMenu(fileName = "New Wizard", menuName = "Wizard", order = 51)] 
    public class WizardData : UnitData
    {
    
        [SerializeField] 
        protected int[] maxMana = new int[3] {100, 100, 100};
        
        public int[] MaxMana => maxMana;
    }
}
