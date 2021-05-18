using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject hpBar;

    public float cooldown = 5.0f;
    private int _hp;
    public int maxHp = 45;
    public int coins = 3;
    public int damage = 5;
    public GameObject damageUIContainer;
    public GameObject damagePrefab;

    private Coroutine _damageCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        _hp = maxHp;
        hpBar.GetComponent<Slider>().value = (float)_hp / maxHp;
        _damageCoroutine = StartCoroutine(DamageThread());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DamageThread()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2);
            GameObject.FindWithTag("Wizard").GetComponent<Wizard>().TakeDamage(damage);
        }
    }
    
    private IEnumerator Die()
    {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Die");
            transform.parent.GetComponent<SpawnEnemy>().SpawnCoins(coins);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            transform.SetParent(null);
            Destroy(gameObject);
    }
    
    public void TakeDamage(int damage)
    {
        GameObject damageUI = Instantiate(damagePrefab, damageUIContainer.transform.position, Quaternion.identity);
        damageUI.transform.SetParent(damageUIContainer.transform);
        damageUI.transform.localScale = new Vector3(1, 1, 1);
        damageUI.GetComponent<Text>().text = damage.ToString();
        _hp = Math.Max(0, _hp - damage);
        hpBar.GetComponent<Slider>().value = (float)_hp / maxHp;
        if (_hp == 0)
        {
            StartCoroutine(Die());
        }
    }
}
