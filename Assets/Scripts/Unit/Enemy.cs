using System.Collections;
using UnitData;
using UnityEngine;

namespace Unit
{
    public class Enemy : Unit
    {
        private int Coins => (int) (((EnemyData)data).Coins * (1 + 0.05f * Level));
        public int Level { get; set; } = 1;
        
        protected new void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = ((EnemyData)data).Color;
        }

        protected override IEnumerator Die()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            transform.parent.GetComponent<SpawnEnemy>().SpawnCoins(Coins);
            Delete();
        }

        public override int GetLevel()
        {
            return Level;
        }

        public override string GetTargetType(bool summonExist)
        {
            if (summonExist)
            {
                return "Summon";
            }
            return "Wizard";
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
