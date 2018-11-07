using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Item")]
public class ItemData : ActionData
{
    protected override IEnumerator ExecuteActionInternal(EntityData instigator, IEnumerable<EntityData> targets)
    {
        throw new System.NotImplementedException();
    }
}
