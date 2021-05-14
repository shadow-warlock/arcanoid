using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject wizard;
    public GameObject hpBar;

    public float cooldown = 5.0f;
    private int _hp;
    public int maxHp = 45;
    public int damage = 5;

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
            GetComponent<Animator>().SetTrigger("Attack");
            wizard.GetComponent<Wizard>().TakeDamage(damage);
        }
    }
}
