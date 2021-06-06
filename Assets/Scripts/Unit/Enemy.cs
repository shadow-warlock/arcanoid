using System.Collections;
using UnitData;
using UnityEngine;
using UnityEngine.UI;

namespace Unit
{
    public class Enemy : Unit
    {
        private int Coins => (int) (3 * (1 + 0.05f * Level));
        public int Level { get; set; } = 1;
        
        protected new void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = ((EnemyData)data).Color;
            statusPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Level.ToString();
        }

        protected override IEnumerator Die()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Die");
            transform.parent.GetComponent<SpawnEnemy>().SpawnCoins(Coins);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Delete();
        }

        protected override string GetTargetType(bool summonExist)
        {
            if (summonExist)
            {
                return Random.Range(1, 100) > 50 ? "Wizard" : "Summon";
            }
            return "Wizard";
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
