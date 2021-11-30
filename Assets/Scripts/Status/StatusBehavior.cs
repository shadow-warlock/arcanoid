using System;
using UnityEngine;
using UnityEngine.UI;

namespace Status
{
    public class StatusBehavior : MonoBehaviour
    {
        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;
                _status.OnDelete += OnStatusDelete;
                _status.OnTick += OnStatusTick;
                GetComponent<Image>().sprite = _status.Icon;
            }
        }
        private Status _status;
        public Action<string, bool> OnTick;
        
        private void OnStatusDelete()
        {
            transform.SetParent(null);
            Destroy(gameObject);
            _status.OnDelete -= OnStatusDelete;
            _status.OnTick -= OnStatusTick;
        }

        private void OnStatusTick()
        {
            GetComponent<Image>().fillAmount = 1.0f - (float) Status.CurrentTime / Status.Time;
            OnTick(Status.Text, false);
        }
    }
}