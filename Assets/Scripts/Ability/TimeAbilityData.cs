using Unit;
using UnityEngine;

[CreateAssetMenu(fileName = "New Time Ability", menuName = "Time Ability", order = 51)] 
public class TimeAbilityData : AbilityData
{

    [SerializeField]
    private float cooldown;


    public float Cooldown => cooldown;

    protected override ManaType? getType()
    {
        return null;
    }
}
