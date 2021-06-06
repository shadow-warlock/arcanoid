using System;
using System.Collections;
using System.Linq;
using Ability;
using UnityEngine;
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

    public int MAXMana(int type)
    {
        return (int) ((1 + _levels[type] * 0.05) * _maxMana[type]);
    }

    private int _coins = 0;
    private int[] _levels = new int[3] {0, 0, 0};
    private int _exp = 0;
    private int[] _mana = new int[3] {0, 0, 0};
    public Ability.ManaAbility[] abilities;
    public GameObject manaBarsContainer;
    public GameObject coinIndicator;
    public GameObject castContainer;
    public GameObject levelContainer;
    public GameObject levelUpPanel;
    private GameObject _gameController;
    public GameObject levelUpButton;
    private int[] _maxMana = new int[3] {100, 100, 100};


    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        for (int i = 0; i < _mana.Length; i++)
        {
            UpdateManaUI(i);
        }

        _gameController = GameObject.FindWithTag("GameController");
        coinIndicator.GetComponent<Text>().text = _coins.ToString();
    }


    private void UpdateManaUI(int index)
    {
        GameObject manaBar = manaBarsContainer.transform.GetChild(index).gameObject;
        manaBar.GetComponent<Slider>().value = (float) _mana[index] / MAXMana(index);
        castContainer.transform.GetChild(index).GetComponent<Button>().interactable =
            _mana[index] >= abilities[index].ManaCost;
        castContainer.transform.GetChild(index).GetChild(0).GetComponent<Image>().sprite = abilities[index].Icon;
    }

    public void AddMana(ManaType type, int count)
    {
        int manaIndex = (int) type;
        _mana[manaIndex] = Math.Min(_mana[manaIndex] + count, MAXMana(manaIndex));
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
        _gameController.GetComponent<GameController>().GameOver("Персонаж погиб");
    }

    protected override string GetTargetType()
    {
        return "Enemy";
    }

    protected override bool CanCast(Ability.Ability ability)
    {
        return ability switch
        {
            TimeAbility _ => true,
            ManaAbility manaAbility => HasMana(manaAbility.ManaType, manaAbility.ManaCost),
            _ => false
        };
    }

    protected override void PreCastDoing(Ability.Ability ability)
    {
        if (ability is ManaAbility manaAbility)
        {
            RemoveMana(manaAbility.ManaType, manaAbility.ManaCost);
        }
    }
    
    protected override float GetModificator(Ability.Ability ability)
    {
        return ability switch
        {
            TimeAbility _ => 1 + 0.1f * GetSuperLevel(),
            ManaAbility manaAbility => 1 + 0.05f * _levels[(int) manaAbility.ManaType],
            _ => 1
        };
    }

    protected override float GetMaxHpModificator()
    {
        return 1.05f * GetSuperLevel();
    }


    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coin"))
        {
            _coins++;
            coinIndicator.GetComponent<Text>().text = _coins.ToString();
            _exp++;
            if (_exp < GetExpLevelUp())
            {
                _exp++;
                levelContainer.transform.GetChild(1).GetComponent<Slider>().value = (float) _exp / GetExpLevelUp();
            }

            if (_exp >= GetExpLevelUp())
            {
                levelUpButton.SetActive(true);
            }

            Destroy(collider.gameObject);
        }
    }

    public void StartLevelUp()
    {
        Time.timeScale = 0;
        levelUpPanel.SetActive(true);
    }

    public void LevelUp(int type)
    {
        _levels[type]++;
        UpdateManaUI(type);
        levelContainer.transform.GetChild(0).GetComponent<Text>().text = GetSuperLevel().ToString();
        _exp = 0;
        levelContainer.transform.GetChild(1).GetComponent<Slider>().value = (float) _exp / GetExpLevelUp();
        TakeHeal((int) (MAXHp * 0.2f));
        Time.timeScale = 1;
        levelUpPanel.SetActive(false);
        levelUpButton.SetActive(false);
    }

    public int GetSuperLevel()
    {
        return _levels.Sum() + 1;
    }

    private int GetExpLevelUp()
    {
        return GetSuperLevel() * 10;
    }

    public void Cast(int index)
    {
        UseAbility(abilities[index]);
    }
}