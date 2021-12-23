using UnityEngine;


public abstract class StatusData : ScriptableObject
{
    [SerializeField]
    private Sprite icon;
    
    [SerializeField]
    private bool isView = true;
    public Sprite Icon => icon;

    public bool IsView => isView;
}
