using UnityEngine;
using UnityEngine.UI;

namespace UnitUI
{
    public abstract class UnitUIListener <T> :  MonoBehaviour where T : Unit.Unit
    {
        protected T unit;

        public T Unit
        {
            set
            {
                unit = value;
                ConnectToUnit();
            }
        }

        [SerializeField]
        private GameObject damagePrefab;
        
        [SerializeField]
        private GameObject statusPanel;
        
        [SerializeField]
        private GameObject statusPrefab;

        protected virtual void ConnectToUnit()
        {
            if (unit != null)
            {
                unit.OnCreate += OnCreate;
                unit.OnDamage += OnDamage;
                unit.OnDelete += OnDelete;
                unit.OnUpdate += OnUpdate;
                unit.OnAddStatus += OnAddStatus;   
            }
        }

        protected void OnCreate()
        {
            statusPanel.SetActive(true);
            OnUpdate();
        }

        protected void OnUpdate()
        {
            UpdateLevelUI();
            UpdateHpBar();
        }

        protected void OnDelete()
        {
            Transform damageUIContainer = statusPanel.transform.GetChild(3);
            for(int i=0; i < damageUIContainer.childCount; i++)
            {
                Transform damageTransform = damageUIContainer.GetChild(i);
                damageTransform.SetParent(null);
                Destroy(damageTransform.gameObject);
            }
            Transform statusesContainer = statusPanel.transform.GetChild(2);
            for(int i=0; i < statusesContainer.childCount; i++)
            {
                Transform statusTransform = statusesContainer.GetChild(i);
                statusTransform.SetParent(null);
                Destroy(statusTransform.gameObject);
            }
            statusPanel.SetActive(false);
        }

        protected void OnAddStatus(IStatus status)
        {
            if (status.IsView)
            {
                Transform statusesContainer = statusPanel.transform.GetChild(2);
                GameObject statusObj = Instantiate(statusPrefab, statusesContainer.position, Quaternion.identity, statusesContainer);
                StatusBehavior statusUI = statusObj.GetComponent<StatusBehavior>();
                statusUI.Status = status;
            }
        }


        protected void OnDamage(Unit.Unit damager, int damage, bool isPositive)
        {
            Transform damageUIContainer = statusPanel.transform.GetChild(3);
            GameObject damageUI = Instantiate(damagePrefab, damageUIContainer.position, Quaternion.identity);
            damageUI.transform.SetParent(damageUIContainer);
            damageUI.transform.localScale = new Vector3(1, 1, 1);
            damageUI.GetComponent<Text>().text = damage.ToString();
            if (isPositive)
            {
                damageUI.GetComponent<Text>().color = new Color(0.25f, 1f, 0.24f);
            }
            UpdateHpBar();
        }
        
        protected void UpdateHpBar()
        {
            statusPanel.transform.GetChild(1).GetComponent<Slider>().value = (float)unit.Hp / unit.MAXHp;
        }

        protected void UpdateLevelUI()
        {
            statusPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = unit.GetLevel().ToString();
        }
    }
}