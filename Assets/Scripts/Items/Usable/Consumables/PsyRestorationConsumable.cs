using UnityEngine;
using System.Collections;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Psy Restore Item", menuName = "Items/Consumable/Psy Restoration", order = 1)]
public class PsyRestorationConsumable : Consumable
{
    // Objects that restore psynergy points to the player.
    [Header("Psynergy restoration settings")]
    public HealthRestorationMode restoreMode;
    public float PsyRestored = 10.0f;

    [RangeAttribute(0.0f, 100.0f)]
    public float percentageRestored = 10.0f;
}