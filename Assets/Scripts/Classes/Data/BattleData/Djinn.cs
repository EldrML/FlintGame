using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Battle/Djinn")]
public class Djinn : Skill {

    [System.Serializable]
    public enum DjinnState { STATE_SET, STATE_STANDBY, STATE_SUMMON }

    [System.Serializable]
    public enum DjinnElement { ELEMENT_EARTH, ELEMENT_FIRE, ELEMENT_WATER, ELEMENT_AIR }

    public DjinnState State { get; set; }
    public int Cooldown { get; set; }
}
