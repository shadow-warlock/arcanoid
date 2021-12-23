using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New On End Status", menuName = "OnEndStatus", order = 51)]
public class OnEndStatusData : CountStatusDecoratorData
{
    [SerializeField] 
    private List<Effect> targetEffects = new List<Effect>();
    
    [SerializeField] 
    private List<Effect> casterEffects = new List<Effect>();

    public List<Effect> TargetEffects => targetEffects;

    public List<Effect> CasterEffects => casterEffects;
}
