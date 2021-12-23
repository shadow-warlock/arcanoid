using Unit;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mana Ability", menuName = "Mana Ability", order = 51)] 
public class ManaAbilityData : AbilityData
{

    [SerializeField] 
    protected int manaCost;

    [SerializeField] 
    protected ManaType manaType;

    [SerializeField]
    protected Sprite icon;
    
    public int ManaCost => manaCost;
    public ManaType ManaType => manaType;
    public Sprite Icon => icon;


    protected override ManaType? getType()
    {
        return manaType;
    }
}
