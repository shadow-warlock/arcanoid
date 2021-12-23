using System.Collections;
using UnityEngine;

namespace Unit
{
    public class Summon : Unit
    {
        public int Level { get; set; } = 1;
        public Wizard owner { get; set; }
        
        protected new void Start()
        {
            base.Start();
        }

        protected override IEnumerator Die()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Delete();
        }

        public override int GetLevel()
        {
            return Level;
        }

        public override string GetTargetType(bool summonExist)
        {
            return "Enemy";
        }

        public override float GetModificator(ManaType? type)
        {
            return 1 + 0.05f * Level;
        }

        protected override float GetMaxHpModificator()
        {
            return 1 + 0.05f * Level;
        }

        public override bool CanCast(AbilityData abilityData)
        {
            return true;
        }

        public override void PreCastDoing(AbilityData abilityData)
        {
        
        }
        
    }
}
