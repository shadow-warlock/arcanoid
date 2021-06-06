using System.Collections.Generic;
using Ability;
using UnityEngine;

namespace UnitData
{
    [CreateAssetMenu(fileName = "New Summon", menuName = "Summon", order = 51)] 
    public class SummonData : UnitData
    {
    
        [SerializeField] 
        protected int baseLevel;
        
        public int BaseLevel => baseLevel;
        
    }
}
