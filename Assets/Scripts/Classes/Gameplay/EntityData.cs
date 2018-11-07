using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "Gameplay/Entity")]
public class EntityData : ScriptableObject, IHasName
{
    [SerializeField]
    private AttackActionData _attack;
    [SerializeField]
    private DefendActionData _defend;
    
    [SerializeField]
    private List<PsyData> _psy = new List<PsyData>();
    [SerializeField]
    private List<DjinnData> _djinn = new List<DjinnData>();
    [SerializeField]
    private List<ItemData> _item = new List<ItemData>();
    [SerializeField]
    private List<SummonData> _summon = new List<SummonData>();

    public AttackActionData Attack { get { return _attack; } }
    public DefendActionData Defend { get { return _defend; } }

    public event Action HealthChanged;
    public event Action PsyPointsChanged;
    public event Action ActingStateChanged;

    public string Name { get { return _name; } }

    [SerializeField]
    private string _name = "NameHere";

    [SerializeField]
    private int _psyPoints;
    [SerializeField]
    private int _healthPoints;

    private bool _isActing;
    public bool IsActing { get { return _isActing; } set { _isActing = value; ActingStateChanged?.Invoke(); } }

    public int PsyPoints { get { return _psyPoints; } set { _psyPoints = value; PsyPointsChanged?.Invoke(); } }
    public int HealthPoints { get { return _healthPoints; } set { _healthPoints = value; HealthChanged?.Invoke(); } }

    public IReadOnlyList<ActionData> AvailableActions(ActionData.ActionType actionType)
    {
        switch (actionType)
        {
            case ActionData.ActionType.Psy:
                return _psy;
            case ActionData.ActionType.Djinn:
                return _djinn;
            case ActionData.ActionType.Summon:
                return _summon;
            case ActionData.ActionType.Inventory:
                return _item;
        }

        throw new Exception("ERROR");
    }
}
