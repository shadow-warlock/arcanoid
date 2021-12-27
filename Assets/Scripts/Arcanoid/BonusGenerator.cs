using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator: MonoBehaviour
{
    [SerializeField]
    private List<Bonus> bonuses;
    private void OnDestroy()
    {
        foreach (Bonus bonus in bonuses)
        {
            if (bonus.Chance >= Random.Range(0.0f, 1.0f))
            {
                Bonus createdBonus = Instantiate(bonus, transform.parent.parent);
                createdBonus.transform.localPosition = transform.parent.localPosition;
                createdBonus.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
