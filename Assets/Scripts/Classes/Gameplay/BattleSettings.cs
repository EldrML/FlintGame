using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Battle Settings")]
public class BattleSettings : ScriptableObject
{
    [SerializeField]
    private List<EntityData> _enemies;

    public List<EntityData> Enemies { get { return _enemies; } }
}
