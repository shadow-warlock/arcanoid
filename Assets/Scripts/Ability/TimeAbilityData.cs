using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(fileName = "New Time Ability", menuName = "Time Ability", order = 51)] 
    public class TimeAbilityData : AbilityData
    {

        [SerializeField]
        private float cooldown;
    

        public float Cooldown => cooldown;

    }
}
