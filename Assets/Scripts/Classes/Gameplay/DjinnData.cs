using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Djinn")]
public class DjinnData : ActionData
{
    public enum DjinnState { Set, Standby, Summon }

    public enum DjinnElement { Earth, Fire, Water, Air }

    public DjinnState State { get; set; }
    public int Cooldown { get; set; }

    protected override IEnumerator ExecuteActionInternal(EntityData instigator, IEnumerable<EntityData> targets)
    {
        throw new System.NotImplementedException();
    }
}
