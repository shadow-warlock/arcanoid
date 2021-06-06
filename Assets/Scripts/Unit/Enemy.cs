using System.Collections;
using UnityEngine;

public class Enemy : Unit
{
    private int Coins => (int) (3 * 1.05f * Level);

    public int Level { get; set; } = 1;


    protected override IEnumerator Die()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Die");
        transform.parent.GetComponent<SpawnEnemy>().SpawnCoins(Coins);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        transform.SetParent(null);
        Destroy(gameObject);
    }

    protected override string GetTargetType()
    {
        return "Wizard";
    }

    protected override float GetModificator(Ability.Ability ability)
    {
        return 1.1f * Level;
    }

    protected override float GetMaxHpModificator()
    {
        return 1.1f * Level;
    }
    
    protected override bool CanCast(Ability.Ability ability)
    {
        return true;
    }

    protected override void PreCastDoing(Ability.Ability ability)
    {
        
    }
}
