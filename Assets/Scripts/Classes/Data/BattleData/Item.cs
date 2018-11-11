using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Battle/Item")]
public class ItemSkill : Skill {

    [SerializeField]
    private int _price;

    public int Cost { get { return _price; } }
}
