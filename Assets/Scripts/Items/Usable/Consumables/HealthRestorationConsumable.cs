using UnityEngine;
using System.Collections;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Health Restore Item", menuName = "Items/Consumable/Health Restoration", order = 1)]
public class HealthRestorationConsumable : Consumable
{
    // Objects that restore health to the player.
    [Header("Health restoration settings")]
    public HealthRestorationMode restoreMode;
    public float healthRestored = 10.0f;

    [RangeAttribute(0.0f, 100.0f)]
    public float percentageRestored = 10.0f;
}