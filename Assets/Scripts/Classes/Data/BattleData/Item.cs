using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Battle/Item")]
public class Item : Skill {

    [SerializeField]
    private int _price;

    public int Cost { get { return _price; } }
}
