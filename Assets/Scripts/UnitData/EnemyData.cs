using UnityEngine;

namespace UnitData
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy", order = 51)] 
    public class EnemyData : UnitData
    {
    
        [SerializeField] 
        protected Color color;
        
        [SerializeField] 
        protected int coins;
        
        public Color Color => color;

        public int Coins => coins;
    }
}
