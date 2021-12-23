using System;
using Unit;
using UnitData;
using UnityEngine;

[CreateAssetMenu(fileName = "New Summon Effect", menuName = "SummonEffect", order = 51)]
public class SummonEffect : Effect
{
    
    [SerializeField] 
    private SummonData data;
    
    public override void Do(Unit.Unit caster, Unit.Unit finalTarget, float multiplier)
    {
        Wizard wizard = caster as Wizard;
        if (wizard == null)
        {
            throw new Exception("Not wizard cannot cast");
        }
        
        if (wizard.Summon != null)
        {
            wizard.Summon.Delete();
        }

        GameObject summonObject = Instantiate(data.Prefab, wizard.summonSpawner.transform.position,
            Quaternion.identity, wizard.summonSpawner.transform);
        Summon summon = summonObject.GetComponent<Summon>();
        summon.owner = wizard;
        summon.Level = (int) ((data.BaseLevel + wizard.GetLevel()) * multiplier);
        wizard.Summon = summon;
        wizard.OnSummon?.Invoke(summon);
    }
}
