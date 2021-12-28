using System;
using System.Collections;
using System.Linq;
using UnitData;
using UnityEngine;

namespace Unit
{
    public class Wizard : Unit
    {
        public Action<Summon> OnSummon;
        public Action<int, float, bool> OnUpdateMana;
        public Action<int, int, int> OnUpdateExpAndCoins;

        public override int MAXHp => base.MAXHp + ShopStore.GetInstance().GetProductCount(ShopStore.Product.MaxHp) * 50;


        public int MAXMana(int type)
        {
            ShopStore.Product[] shopUpgrades =
            {
                ShopStore.Product.MaxMana1,
                ShopStore.Product.MaxMana2,
                ShopStore.Product.MaxMana3,
            };
            return (int) ((1 + _levels[type] * 0.05) * ((WizardData) data).MaxMana[type]) + ShopStore.GetInstance().GetProductCount(shopUpgrades[type]) * 10;
        }

        private int _coins = 0;
        private int[] _levels = new int[3] {0, 0, 0};
        private int _exp = 0;
        private int[] _mana = new int[3] {0, 0, 0};
        public Summon Summon { get; set; }

        public ManaAbilityData[] abilities;
        public GameObject levelUpPanel;
        public GameObject summonSpawner;

        protected new void Start()
        {
            base.Start();
            for (int i = 0; i < _mana.Length; i++)
            {
                UpdateMana(i);
            }

            OnUpdateExpAndCoins(_exp, GetExpLevelUp(), _coins);
        }

        public void GainExp(int exp)
        {
            _exp += exp;
            OnUpdateExpAndCoins(_exp, GetExpLevelUp(), _coins);
        }
        
        public void GainCoins(int coins)
        {
            _coins += coins;
            OnUpdateExpAndCoins(_exp, GetExpLevelUp(), _coins);
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
        }

        public override string GetTargetType(bool summonExist)
        {
            return "Enemy";
        }

        public override bool CanCast(AbilityData abilityData)
        {
            return abilityData switch
            {
                TimeAbilityData _ => true,
                ManaAbilityData manaAbility => HasMana(manaAbility.ManaType, manaAbility.ManaCost),
                _ => false
            };
        }

        public override void PreCastDoing(AbilityData abilityData)
        {
            if (abilityData is ManaAbilityData manaAbility)
            {
                RemoveMana(manaAbility.ManaType, manaAbility.ManaCost);
            }
        }

        public override float GetModificator(ManaType? type)
        {
            return type switch
            {
                ManaType.Blue => 1 + 0.1f * GetLevel() + 0.1f * _levels[(int) type],
                ManaType.Turquoise => 1 + 0.1f * GetLevel() + 0.1f * _levels[(int) type],
                ManaType.Purple => 1 + 0.1f * GetLevel() + 0.1f * _levels[(int) type],
                _ => 1 + 0.1f * GetLevel()
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
            TakeHeal(this, (int) (MAXHp * 0.2f));
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