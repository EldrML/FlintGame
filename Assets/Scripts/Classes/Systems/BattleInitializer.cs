using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoSingleton<BattleInitializer>
{

    protected override BattleInitializer GetSingletonInstance { get { return this; } }

    private void Start()
    {
        BattleController.Instance.StateChanged += StateChanged;
    }

    private void StateChanged(BattleController.States oldState, BattleController.States newState)
    {
        if (newState != BattleController.States.InitBattle) return;
        //This will later handle things like scene transitions and initial camera movement
        BattleController.Instance.SetState(BattleController.States.MainMenu);
    }
}
