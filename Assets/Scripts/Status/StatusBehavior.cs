using UnityEngine;
using UnityEngine.UI;

public class StatusBehavior : MonoBehaviour
{
    public IStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            _status.OnDelete += OnStatusDelete;
            _status.OnTick += OnStatusTick;
            GetComponent<Image>().sprite = _status.Icon;
        }
    }
    private IStatus _status;
    
    private void OnStatusDelete()
    {
        _status.OnDelete -= OnStatusDelete;
        _status.OnTick -= OnStatusTick;
        transform.SetParent(null);
        Destroy(gameObject);

    }

    private void OnStatusTick(float progress)
    {
        GetComponent<Image>().fillAmount = 1 - progress;
    }
}
