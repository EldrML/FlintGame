using UnityEngine;
using System.Collections;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Usable Item", menuName = "Items/Usable/Usable Item", order = 1)]
public class UsableItem : Item
{
    // An usable item is an item that has an effect on the world, the entire party, or just an item
    // you can use and triggers some animation or cool effect.
    // For example: In Golden Sun a Sacred Feather would be a UsableItemx
    public bool onlyForBattle = false;
}