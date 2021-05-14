using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject wizard;

    public float cooldown = 5.0f;
    public int hp = 45;
    public int damage = 5;

    private Coroutine _damageCoroutine;
    // Start is called before the first frame update
    void Start()
    {
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
            yield return new WaitForSeconds(5);
            GetComponent<Animator>().SetTrigger("Attack");
            wizard.GetComponent<Wizard>().TakeDamage(damage);
        }
    }
}
