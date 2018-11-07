using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Summon")]
public class SummonData : ActionData
{
    public HashSet<DjinnData.DjinnElement> Requirements { get; set; }

    protected override IEnumerator ExecuteActionInternal(EntityData instigator, IEnumerable<EntityData> targets)
    {
        throw new System.NotImplementedException();
    }
}
