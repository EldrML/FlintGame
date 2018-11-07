using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoSingleton<EnemyController>
{
    protected override EnemyController GetSingletonInstance { get { return this; } }

    private Queue<BattleController.TargetedAction> _enemyActions = new Queue<BattleController.TargetedAction>();

    public Queue<BattleController.TargetedAction> EnemyActions { get { return _enemyActions; } } 

    public void HandleEnemies(IEnumerable<EntityData> enemies)
    {
        _enemyActions.Clear();

        //Add enemy actions here
        foreach (var enemy in enemies)
        {
            var action = new BattleController.TargetedAction();
            action.Instigator = enemy;
            action.Action = enemy.Attack;
            action.Targets.Add(PartyController.Instance.PartyMembers[Random.Range(0, PartyController.Instance.PartyMembers.Count)]);
            _enemyActions.Enqueue(action);
        }

        BattleController.Instance.SetState(BattleController.States.ExecuteActions);
    }
}
