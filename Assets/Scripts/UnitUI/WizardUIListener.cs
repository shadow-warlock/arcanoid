using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace UnitUI
{
    public class WizardUIListener : UnitUIListener<Wizard>
    {
        [SerializeField] private GameObject manaBarsContainer;

        [SerializeField] private GameObject castContainer;

        [SerializeField] private GameObject coinIndicator;

        [SerializeField] private GameObject expContainer;
        
        [SerializeField] private GameObject levelUpButton;

        protected override void ConnectToUnit()
        {
            if (unit != null)
            {
                base.ConnectToUnit();
                unit.OnUpdateMana += OnUpdateMana;
                unit.OnUpdateExpAndCoins += OnUpdateExpAndCoins;
                for (int i = 0; i < unit.abilities.Length; i++)
                {
                    castContainer.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                        unit.abilities[i].Icon;
                }
            }
        }

        private void OnUpdateMana(int index, float percent, bool canCast)
        {
            GameObject manaBar = manaBarsContainer.transform.GetChild(index).gameObject;
            manaBar.GetComponent<Slider>().value = percent;
            castContainer.transform.GetChild(index).GetComponent<Button>().interactable = canCast;
        }

        private void OnUpdateExpAndCoins(int exp, int maxExp, int coins)
        {
            coinIndicator.GetComponent<Text>().text = coins.ToString();
            if (exp < maxExp)
            {
                expContainer.transform.GetChild(0).GetComponent<Slider>().value = (float) exp / maxExp;
                levelUpButton.SetActive(false);
            }
            else
            {
                expContainer.transform.GetChild(0).GetComponent<Slider>().value = 1;
                levelUpButton.SetActive(true);
            }
        }
        
        public static Color GetColor(Wizard.ManaType type)
        {
            switch (type)
            {
                case Wizard.ManaType.Purple:
                    return new Color(0.64f, 0.35f, 1f);
                case Wizard.ManaType.Turquoise:
                    return new Color(0.31f, 0.84f, 0.77f);
                case Wizard.ManaType.Blue:
                    return new Color(0.3f, 0.53f, 1f);
            }

            return new Color();
        }
    }
}