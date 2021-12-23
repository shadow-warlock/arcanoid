using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Time Status", menuName = "TimeStatus", order = 51)]
public class TimeStatusData : CountStatusData
{
    [SerializeField]
    private float tickTime;
    
    [SerializeField] 
    private List<Effect> onTickEffects = new List<Effect>();

    public float TickTime => tickTime;

    public List<Effect> OnTickEffects => onTickEffects;

}
