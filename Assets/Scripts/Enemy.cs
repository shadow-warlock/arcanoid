using System.Collections;
using UnityEngine;

public class Enemy : Unit
{

    public int coins = 3;


    protected override IEnumerator Die()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Die");
        transform.parent.GetComponent<SpawnEnemy>().SpawnCoins(coins);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        transform.SetParent(null);
        Destroy(gameObject);
    }

    protected override string GetTargetType()
    {
        return "Wizard";
    }
}
