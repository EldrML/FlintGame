using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Items/Generic Item", fileName = "item", order = 1)]
public class Item : ScriptableObject
{
    // This is a base Item. It represents an Item in the game that does nothing.
    // For example: A game ticket would be an Item. An object for a quest would be an Item.
    // This kind of items does not admit any interactions. There are not usable or equipable.
    [Header("General item settings")]
    public string objectName = "New Item";

    [MultilineAttribute()]
    public string description = "This is an item";

    public Sprite sprite;

    [RangeAttribute(0, 99)]
    public int limit = 99;

    public int buyValue = 0;
    public int sellValue = 0;
    public bool canItBeSold = true;

}