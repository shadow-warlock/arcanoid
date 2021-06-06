using System.Collections.Generic;
using Ability;
using UnityEngine;

namespace UnitData
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy", order = 51)] 
    public class EnemyData : UnitData
    {
    
        [SerializeField] 
        protected Color color;
        
        public Color Color => color;
    }
}
