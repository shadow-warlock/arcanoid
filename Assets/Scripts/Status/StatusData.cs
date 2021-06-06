using System.Collections.Generic;
using Ability;
using UnityEngine;

namespace Status
{
    [CreateAssetMenu(fileName = "New Status", menuName = "Status", order = 51)] 
    public class StatusData : ScriptableObject
    {
        public enum StatusType
        {
            Damage,
            Heal,
            CastCooldown,
            Silent,
            Disarm,
            Revival
        }
    
        [SerializeField]
        protected StatusType type;
    
        [SerializeField]
        protected int time;
    
        [SerializeField]
        protected float tickSize;
    
        [SerializeField]
        protected Sprite icon;
    
        [SerializeField]
        protected float power;
        
        [SerializeField]
        protected string text;
        
        [SerializeField]
        protected bool increasePower;
        
        [SerializeField]
        protected List<StatusAbility> abilities = new List<StatusAbility>();
    
        public StatusType Type => type;
        public int Time => time;
        public float TickSize => tickSize;
        public float Power => power;
        public Sprite Icon => icon;
        public string Text => text;
        public bool IncreasePower => increasePower;
        public List<StatusAbility> Abilities => abilities;
    }
}
