using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Battle/Summon")]
public class Summon : Skill
{
    [SerializeField]
    public Djinn.DjinnElement[] Requirements;
}
