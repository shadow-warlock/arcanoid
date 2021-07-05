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

        protected override string GetTargetType(bool summonExist)
        {
            return "Enemy";
        }

        protected override float GetModificator(Ability.Ability ability)
        {
            return 1 + 0.05f * Level;
        }

        protected override float GetMaxHpModificator()
        {
            return 1 + 0.05f * Level;
        }
    
        protected override bool CanCast(Ability.Ability ability)
        {
            return true;
        }

        protected override void PreCastDoing(Ability.Ability ability)
        {
        
        }
        
    }
}
