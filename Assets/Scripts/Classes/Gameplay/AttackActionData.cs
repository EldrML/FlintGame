using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Attack")]
public class AttackActionData : ActionData
{
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
