using UnitData;
using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(fileName = "New Summon Ability", menuName = "Summon Ability", order = 51)] 
    public class SummonAbility : ManaAbility
    {
        [SerializeField] 
        protected SummonData summon;
        
        public SummonData Summon => summon;
    }
}