using System;
using Ability;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Status
{
    public class Status : MonoBehaviour
    {
        public StatusData Data { get; set; }
        public Coroutine Coroutine { get; set; }
        private int _currentTime;
        public float Coefficient { get; set; }
        public Ability.Ability Ability { get; set; }
        public Unit.Unit Caster { get; set; }
        public Unit.Unit Target { get; set; }
        private void Start()
        {
            _currentTime = 1;
            GetComponent<Image>().sprite = Data.Icon;
        }

        public void Tick()
        {
            switch (Type)
            {
                case StatusData.StatusType.Damage:
                    Target.TakeDamage((int) (Power * Random.Range(0.9f, 1.1f)));
                    break;
                case StatusData.StatusType.Heal:
                    Target.TakeHeal((int) (Power * Random.Range(0.9f, 1.1f)));
                    break;
                case StatusData.StatusType.CastCooldown:
                    break;
                case StatusData.StatusType.Silent:
                    break;
                case StatusData.StatusType.Disarm:
                    break;
                case StatusData.StatusType.Revival:
                    foreach (StatusAbility ability in Data.Abilities)
                    {
                        Target.UseAbility(ability);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _currentTime++;
            GetComponent<Image>().fillAmount = 1.0f - (float) _currentTime / Data.Time;
        }

        public void Delete()
        {
            StopCoroutine(Coroutine);
            transform.SetParent(null);
            Destroy(gameObject);
        }

        public bool isEnd()
        {
            return Data.Time < _currentTime;
        }

        public StatusData.StatusType Type => Data.Type;
        public int Time => Data.Time;
        public int CurrentTime => _currentTime;
        public float TickSize => Data.TickSize;
        public float Power => Data.Power * (Data.IncreasePower ? Coefficient : 1 / Coefficient);
        public Sprite Icon => Data.Icon;
        public string Text => Data.Text;
    }
}