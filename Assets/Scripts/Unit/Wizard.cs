using System;
using System.Collections;
using System.Linq;
using Ability;
using UnitData;
using UnityEngine;
using UnityEngine.UI;

namespace Unit
{
    public class Wizard : Unit
    {
        public Action<Summon> OnSummon;
        public Action<int, float, bool> OnUpdateMana;
        public Action<int, int, int> OnUpdateExpAndCoins;

        public override int MAXHp => base.MAXHp + ShopStore.GetInstance().GetProductCount(ShopStore.Product.MaxHp) * 10;

        public enum ManaType
        {
            Purple = 2,
            Turquoise = 1,
            Blue = 0
        }


        public int MAXMana(int type)
        {
            return (int) ((1 + _levels[type] * 0.05) * ((WizardData) data).MaxMana[type]);
        }

        private int _coins = 0;
        private int[] _levels = new int[3] {0, 0, 0};
        private int _exp = 0;
        private int[] _mana = new int[3] {0, 0, 0};
        private Summon _summon;
        public ManaAbilityData[] abilities;
        public GameObject levelUpPanel;
        private GameObject _gameController;
        public GameObject summonSpawner;

        protected new void Start()
        {
            base.Start();
            for (int i = 0; i < _mana.Length; i++)
            {
                UpdateMana(i);
            }

            OnUpdateExpAndCoins(_exp, GetExpLevelUp(), _coins);
            _gameController = GameObject.FindWithTag("GameController");
        }


        private void UpdateMana(int index)
        {
            if (OnUpdateMana != null)
                OnUpdateMana(index, (float) _mana[index] / MAXMana(index), _mana[index] >= abilities[index].ManaCost);
        }

        public void AddMana(ManaType type, int count)
        {
            int manaIndex = (int) type;
            _mana[manaIndex] = Math.Min(_mana[manaIndex] + count, MAXMana(manaIndex));
            UpdateMana(manaIndex);
        }

        public void Summon(SummonAbilityData abilityData)
        {
            if (_summon != null)
            {
                _summon.Delete();
            }

            GameObject summon = Instantiate(abilityData.Summon.Prefab, summonSpawner.transform.position,
                Quaternion.identity, summonSpawner.transform);
            _summon = summon.GetComponent<Summon>();
            _summon.owner = this;
            _summon.Level = (int) (abilityData.Summon.BaseLevel * GetModificator(abilityData));
            if (OnSummon != null) OnSummon(_summon);
        }

        private bool HasMana(ManaType type, int count)
        {
            int manaIndex = (int) type;
            return _mana[manaIndex] - count >= 0;
        }

        private void RemoveMana(ManaType type, int count)
        {
            int manaIndex = (int) type;

            _mana[manaIndex] = Math.Max(_mana[manaIndex] - count, 0);
            UpdateMana(manaIndex);
        }

        protected override IEnumerator Die()
        {
            yield return new WaitForSeconds(1f);
            _gameController.GetComponent<GameController>().GameOver("Персонаж погиб");
        }

        public override string GetTargetType(bool summonExist)
        {
            return "Enemy";
        }

        public override bool CanCast(Ability.AbilityData abilityData)
        {
            return abilityData switch
            {
                TimeAbilityData _ => true,
                ManaAbilityData manaAbility => HasMana(manaAbility.ManaType, manaAbility.ManaCost),
                _ => false
            };
        }

        public override void PreCastDoing(Ability.AbilityData abilityData)
        {
            if (abilityData is ManaAbilityData manaAbility)
            {
                RemoveMana(manaAbility.ManaType, manaAbility.ManaCost);
            }
        }

        public override float GetModificator(Ability.AbilityData abilityData)
        {
            return abilityData switch
            {
                TimeAbilityData _ => 1 + 0.1f * GetLevel(),
                SummonAbilityData summonAbility => 1 + 1 * _levels[(int) summonAbility.ManaType],
                ManaAbilityData manaAbility => 1 + 0.05f * _levels[(int) manaAbility.ManaType],
                _ => 1
            };
        }

        protected override float GetMaxHpModificator()
        {
            return 1.05f * GetLevel();
        }


        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Coin"))
            {
                _coins++;
                _exp++;
                OnUpdateExpAndCoins(_exp, GetExpLevelUp(), _coins);
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
            _exp -= GetExpLevelUp();
            _levels[type]++;
            UpdateMana(type);
            OnUpdateExpAndCoins(_exp, GetExpLevelUp(), _coins);
            if (OnUpdate != null) OnUpdate();
            TakeHeal((int) (MAXHp * 0.2f));
            Time.timeScale = 1;
            levelUpPanel.SetActive(false);
        }

        public override int GetLevel()
        {
            return _levels.Sum() + 1;
        }

        private int GetExpLevelUp()
        {
            return GetLevel() * 10;
        }

        public void Cast(int index)
        {
            UseAbility(abilities[index]);
        }

        public int GetCoins()
        {
            return _coins;
        }
    }
}