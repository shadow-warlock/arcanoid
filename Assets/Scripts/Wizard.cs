using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Wizard : Unit
{
    public enum ManaType
    {
        Purple = 2,
        Turquoise = 1,
        Blue = 0
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
    private int _coins = 0;
    private int[] _mana = new int[3]{0,0,0};
    public Ability[] abilities;
    public GameObject manaBarsContainer;
    public GameObject coinIndicator;
    public GameObject castContainer;
    

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        
        for (int i = 0; i < _mana.Length; i++)
        {
            UpdateManaUI(i);
        }

        coinIndicator.GetComponent<Text>().text = _coins.ToString();
    }
    

    private void UpdateManaUI(int index)
    {
        GameObject manaBar = manaBarsContainer.transform.GetChild(index).gameObject;
        manaBar.GetComponent<Slider>().value = (float) _mana[index] / maxMana;
        castContainer.transform.GetChild(index).GetComponent<Button>().interactable = _mana[index] >= abilities[index].ManaCost;
    }

    public void AddMana(ManaType type, int count)
    {
        int manaIndex = (int) type;
        _mana[manaIndex] = Math.Min(_mana[manaIndex] + count, maxMana);
        UpdateManaUI(manaIndex);
    }
    
    public bool HasMana(ManaType type, int count)
    {
        int manaIndex = (int) type;
        return _mana[manaIndex] - count >= 0;
    }
    
    public void RemoveMana(ManaType type, int count)
    {
        int manaIndex = (int) type;
        
        _mana[manaIndex] = Math.Max(_mana[manaIndex] - count, 0);
        UpdateManaUI(manaIndex);
    }

    protected override IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    protected override string GetTargetType()
    {
        return "Enemy";
    }
    

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coin"))
        {
            _coins++;
            coinIndicator.GetComponent<Text>().text = _coins.ToString();
            Destroy(collider.gameObject);
        }
    }

    public void Cast(int index)
    {
        UseAbility(abilities[index]);
    }
}