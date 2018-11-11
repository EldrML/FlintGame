using UnityEngine;
using System.Collections;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Items/Equipable/Psynergy Item", fileName="Psynergy Giver", order=1)]
public class PsynergyItem : EquipableItem
{
    // A Psynergy Item is an Item that makes the character who has it equipped learn some psynergy.

    // TODO: Implement a psynergy scriptable object for this
    [Header("Psynergy provided settings")]
    // Psynergy boundPsynergy = null;
    public String boundPsynergy = "[Placeholder] A psynergy"; //This is placeholder. Should not be a string.
}