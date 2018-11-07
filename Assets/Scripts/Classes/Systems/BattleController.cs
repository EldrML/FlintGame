using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BattleController : MonoSingleton<BattleController>
{

    public enum States { NoBattle, InitBattle, MainMenu, SelectAction, SelectTargets, EnemyAI, ExecuteActions, FleeBattle, EndBattle }

    protected override BattleController GetSingletonInstance { get { return this; } }
    [SerializeField]
    private SimpleFSM<States> _fsm;

    private List<EntityData> _enemies = new List<EntityData>();
    public IReadOnlyList<EntityData> Enemies { get { return _enemies; } }

    public event SimpleFSM<States>.StateChange StateChanged { add { _fsm.StateChanged += value; } remove { _fsm.StateChanged -= value; } }
    public States CurrentState { get { return _fsm.State; } }

    [SerializeField]
    private int _currentPartyMemberIndex;
    [SerializeField]
    private TargetedAction _currentSelectAction;

    public EntityData CurrentActingPartyMember { get { return PartyController.Instance.PartyMembers.ElementAtOrDefault(_currentPartyMemberIndex); } }

    public event Action ActingPartyMemberChanged;

    private Queue<TargetedAction> _actionQueue = new Queue<TargetedAction>();

    [SerializeField]
    private Transform[] _enemyAnchors;

    [Serializable]
    public class TargetedAction
    {
        public EntityData Instigator { get; set; }
        public ActionData Action { get; set; }
        public List<EntityData> Targets { get; set; } = new List<EntityData>();
    }

    protected override void Awake()
    {
        base.Awake();
        _fsm = new SimpleFSM<States>(this);
    }

    private void Start()
    {
        GameController.Instance.GameStarted += Init;
    }

    private void Init()
    {
        _fsm.StartFSM(States.NoBattle);
    }

    public void SetState(States state)
    {
        if (!_fsm.IsRunning) return;
        _fsm.SetState(state);
    }

    public void StartBattle(BattleSettings battle)
    {
        _enemies.Clear();

        foreach (var e in battle.Enemies)
            _enemies.Add(Instantiate(e));

        SetState(States.InitBattle);
    }

    public void EndBattle()
    {
        SetState(States.EndBattle);
    }

    public void FleeBattle()
    {
        SetState(States.FleeBattle);
        SetState(States.EndBattle);
    }

    public void FightBattle()
    {
        SetState(States.SelectAction);
    }

    public IEnumerable<ActionData> CurrentCharacterActions(ActionData.ActionType type)
    {
        return PartyController.Instance.PartyMembers[_currentPartyMemberIndex].AvailableActions(type);
    }

    public void AddAttackForCurrentCharacter()
    {
        _currentSelectAction.Action = CurrentActingPartyMember.Attack;
        _currentSelectAction.Targets.Clear();
        SetState(States.SelectTargets);
    }

    public void AddDefendForCurrentCharacter()
    {
        _currentSelectAction.Action = CurrentActingPartyMember.Defend;
        _currentSelectAction.Targets.Clear();
        FinaliseAction();
    }

    public void AddActionForCurrentCharacter(ActionData action)
    {
        _currentSelectAction.Action = action;
        SetState(States.SelectTargets);
    }

    public void AddTargetToCurrentAction(EntityData target)
    {
        if (_currentSelectAction == null || _currentSelectAction.Targets.Contains(target)) return;

        _currentSelectAction.Targets.Add(target);

        if (_currentSelectAction.Targets.Count >= _currentSelectAction.Action.TargetCount)
            FinaliseAction();
    }

    public void FinaliseAction()
    {
        _actionQueue.Enqueue(_currentSelectAction);
        _currentPartyMemberIndex++;
        if (_currentPartyMemberIndex < PartyController.Instance.PartyMembers.Count) SetState(States.SelectAction);
        else SetState(States.EnemyAI);

        _currentPartyMemberIndex %= PartyController.Instance.PartyMembers.Count;

        ActingPartyMemberChanged?.Invoke();
    }

    private IEnumerator _ExecuteActions()
    {
        var playerActions = _actionQueue;
        var enemyActions = EnemyController.Instance.EnemyActions;

        foreach (var action in playerActions)
        {
            if (action.Instigator.HealthPoints > 0)
                yield return StartCoroutine(action.Action.ExecuteAction(action.Instigator, action.Targets));
        }

        foreach (var action in enemyActions)
        {
            if (action.Instigator.HealthPoints > 0)
                yield return StartCoroutine(action.Action.ExecuteAction(action.Instigator, action.Targets));
        }

        bool enemyAlive = false;

        var removeList = new List<EntityData>();

        foreach (var enemy in _enemies)
        {
            if (enemy.HealthPoints <= 0)
            {
                removeList.Add(enemy);
            }
            else enemyAlive = true;
        }

        foreach (var remove in removeList)
        {
            _enemies.Remove(remove);
            Destroy(remove);
        }

        SetState(enemyAlive ? States.MainMenu : States.EndBattle);
    }

    //States

    private void _MainMenu_Enter()
    {
        _currentPartyMemberIndex = -1;
        _actionQueue.Clear();
    }

    private void _MainMenu_Exit()
    {
        _currentPartyMemberIndex = 0;
        ActingPartyMemberChanged?.Invoke();
    }

    private void _SelectAction_Enter()
    {
        _currentSelectAction = new TargetedAction()
        {
            Instigator = CurrentActingPartyMember
        };
    }

    private void _EnemyAI_Enter()
    {
        _currentPartyMemberIndex = -1;
        ActingPartyMemberChanged?.Invoke();
        EnemyController.Instance.HandleEnemies(_enemies);
    }

    private void _ExecuteActions_Enter()
    {
        StartCoroutine(_ExecuteActions());
    }

#if UNITY_EDITOR

    private void OnGUI()
    {
        if (!Application.isPlaying || _fsm == null) return;
        GUILayout.TextField(CurrentState.ToString());
    }

#endif

}
