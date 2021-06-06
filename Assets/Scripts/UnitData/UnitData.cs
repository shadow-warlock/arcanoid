using System.Collections.Generic;
using Ability;
using UnityEngine;

namespace UnitData
{
    public abstract class UnitData : ScriptableObject
    {
        [SerializeField]
        protected int maxHp;
    
        [SerializeField]
        protected List<TimeAbility> abilities = new List<TimeAbility>();

        [SerializeField] 
        protected GameObject prefab;
    
        public int MaxHp => maxHp;
    
        public List<TimeAbility> Abilities => abilities;

        public GameObject Prefab => prefab;
    
    }
}
