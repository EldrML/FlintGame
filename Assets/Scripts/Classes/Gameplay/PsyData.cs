using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Psy")]
public class PsyData : ActionData
{
    [SerializeField]
    private int _psyPointCost;

    public int PsyPointCost { get { return _psyPointCost; } }

    protected override IEnumerator ExecuteActionInternal(EntityData instigator, IEnumerable<EntityData> targets)
    {
        foreach (var target in targets)
        {
            Debug.Log(instigator.Name + " used " + Name + " on " + target.Name + " and dealt " + Power + " damage!");
            target.HealthPoints -= Power;
            yield return new WaitForSeconds(1);
        }
    }
}
