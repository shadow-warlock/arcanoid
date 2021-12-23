using System.Collections.Generic;
using UnityEngine;

namespace UnitData
{
    public abstract class UnitData : ScriptableObject
    {
        [SerializeField]
        protected int maxHp;
    
        [SerializeField]
        protected List<TimeAbilityData> abilities = new List<TimeAbilityData>();

        [SerializeField] 
        protected GameObject prefab;
        
        [SerializeField]
        protected List<StatusData> defaultStatuses = new List<StatusData>(); 
    
        public int MaxHp => maxHp;
    
        public List<TimeAbilityData> Abilities => abilities;

        public GameObject Prefab => prefab;

        public List<StatusData> DefaultStatuses => defaultStatuses;
    }
}
