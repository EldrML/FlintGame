using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoSingleton<EnemyController>
{
    protected override EnemyController GetSingletonInstance { get { return this; } }

    private Queue<UnityBattleController.TargetedAction> _enemyActions = new Queue<UnityBattleController.TargetedAction>();

    public Queue<UnityBattleController.TargetedAction> EnemyActions { get { return _enemyActions; } } 

    public void HandleEnemies(IEnumerable<EntityData> enemies)
    {
        _enemyActions.Clear();

        //Add enemy actions here
        foreach (var enemy in enemies)
        {
            var action = new UnityBattleController.TargetedAction();
            action.Instigator = enemy;
            action.Action = enemy.Attack;
            action.Targets.Add(PartyController.Instance.PartyMembers[Random.Range(0, PartyController.Instance.PartyMembers.Count)]);
            _enemyActions.Enqueue(action);
        }

        UnityBattleController.Instance.SetState(UnityBattleController.States.ExecuteActions);
    }
}
