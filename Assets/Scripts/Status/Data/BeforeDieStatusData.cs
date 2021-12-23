using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Before Die Status", menuName = "BeforeDieStatus", order = 51)]
public class BeforeDieStatusData : CountStatusDecoratorData
{
    [SerializeField] 
    private List<Effect> beforeDieEffects = new List<Effect>();
    
    [SerializeField] 
    private List<Effect> beforeDieKillerEffects = new List<Effect>();
    [SerializeField] 
    private bool isSave;
    public List<Effect> BeforeDieEffects => beforeDieEffects;
    
    public List<Effect> BeforeDieKillerEffects => beforeDieKillerEffects;

    public bool IsSave => isSave;

}
