using System;
using System.Collections.Generic;
using Status;
using UnityEngine;

namespace Ability
{
    public abstract class Ability : ScriptableObject
    {
        public enum EffectType
        {
            Damage,
            Heal,
            StatusOnly,
            Summon
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
        protected TargetType target;
    
        [SerializeField] 
        protected string trigger;
    
        [SerializeField] 
        private float damageTime;
        
        [SerializeField] 
        private List<StatusData> targetStatuses = new List<StatusData>();
        
        [SerializeField] 
        private List<StatusData> casterStatuses = new List<StatusData>();
    

        public EffectType Type => type;
        public List<StatusData> TargetStatuses => targetStatuses;
        public List<StatusData> CasterStatuses => casterStatuses;
    
        public int Power => power;
        public TargetType Target => target;
        public String Trigger => trigger;
        public float DamageTime => damageTime;


    }
}
