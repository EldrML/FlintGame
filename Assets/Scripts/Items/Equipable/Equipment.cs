using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Equipment : Item
{
    // An equipment defines properties that define the character gear.
    [Header("Equipment modifier values")]
    public int attackModifier = 0;
    public int defenseModifier = 0;
    public int agilityModifier = 0;
    public int luckModifier = 0;

}