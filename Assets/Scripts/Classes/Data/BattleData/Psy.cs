using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Battle/Psy")]
public class Psy : Skill
{
    [SerializeField]
    private int _cost;

    public int Cost { get { return _cost; } }
    //TODO: prerequsities
}
