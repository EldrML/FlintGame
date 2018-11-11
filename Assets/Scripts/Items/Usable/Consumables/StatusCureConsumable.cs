using UnityEngine;
using System.Collections;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Status Cure Item", menuName = "Items/Consumable/Status Cure", order = 1)]
public class StatusCureConsumable : Consumable
{
    // Objects that cure status to the player.

    // FIXME: This should not be here. Only for testing
    public enum Status
    {
        Delusion,
        Stun,
        Sleep,
        Seal,
        DeathCurse,
        TemporaryStun
    }

    [Header("Status cure settings")]
    public Status[] statusesCured;
}