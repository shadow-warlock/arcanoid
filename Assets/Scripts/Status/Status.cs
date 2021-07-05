using System;
using System.Collections;
using System.Runtime.InteropServices;
using Ability;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Status
{
    public class Status
    {
        public Action OnTick;
        public Action OnDelete;

        private StatusData Data { get; }
        private int _currentTime;
        private float Coefficient { get; }
        private Unit.Unit Caster { get; }
        private Unit.Unit Target { get; }

        
        public Status(StatusData data, Unit.Unit caster, Unit.Unit target, float coefficient)
        {
            Data = data;
            Caster = caster;
            Target = target;
            Coefficient = coefficient;
            _currentTime = 1;
        }

        public IEnumerator Work()
        {
            while (!IsEnd())
            {
                float waitTickSize = 0;
                while (!IsEnd() && waitTickSize < TickSize)
                {
                    waitTickSize += 0.05f;
                    yield return new WaitForSeconds(0.05f);

                }
                Tick();
            }
            Delete();
        }

        public void TickHandler()
        {
            if (!IsEnd())
            {
                Tick();
            }
            if (IsEnd())
            {
                Delete();
            }
        }

        private void Tick()
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
            if (OnTick != null) OnTick();
        }

        public void Stop()
        {
            if (OnDelete != null) OnDelete();
        }
        
        private void Delete()
        {
            Target.OnStatusEnd(this);
            if (OnDelete != null) OnDelete();
        }

        public bool IsEnd()
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