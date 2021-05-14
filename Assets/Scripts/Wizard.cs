using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : MonoBehaviour
{
    public enum ManaType
    {
        Purple = 0,
        Turquoise = 1,
        Blue = 2
    }

    public static Color GetColor(ManaType type)
    {
        switch (type)
        {
            case ManaType.Purple:
                return new Color(0.84f, 0.29f, 1f);
            case ManaType.Turquoise:
                return new Color(0f, 0.83f, 1f);
            case ManaType.Blue:
                return new Color(0f, 0.42f, 1f);
        }

        return new Color();
    }

    public int maxMana = 100;
    public int maxHp = 100;
    private int _hp;
    private int[] _mana = new int[3];
    public GameObject manaBarsContainer;
    public GameObject hpBar;
    public GameObject damageUIContainer;
    public GameObject damagePrefab;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < _mana.Length; i++)
        {
            _mana[i] = 0;
        }

        for (int i = 0; i < manaBarsContainer.transform.childCount; i++)
        {
            GameObject manaBar = manaBarsContainer.transform.GetChild(i).gameObject;
            manaBar.GetComponent<Slider>().value = (float)_mana[i] / maxMana;
        }
        _hp = maxHp;
        hpBar.GetComponent<Slider>().value = (float)_hp / maxHp;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        GameObject damageUI = Instantiate(damagePrefab, damageUIContainer.transform.position, Quaternion.identity);
        damageUI.transform.SetParent(damageUIContainer.transform);
        damageUI.transform.localScale = new Vector3(1, 1, 1);
        damageUI.GetComponent<Text>().text = damage.ToString();
        _hp = Math.Max(0, _hp - damage);
        hpBar.GetComponent<Slider>().value = (float)_hp / maxHp;
    }

    public void AddMana(ManaType type, int count)
    {
        int manaIndex = (int) type;
        _mana[manaIndex] = Math.Min(_mana[manaIndex] + count, maxMana);
        GameObject manaBar = manaBarsContainer.transform.GetChild(manaIndex).gameObject;
        manaBar.GetComponent<Slider>().value = (float)_mana[manaIndex] / maxMana;
    }
}
