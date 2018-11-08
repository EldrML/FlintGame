using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoSingleton<BattleInitializer>
{

    protected override BattleInitializer GetSingletonInstance { get { return this; } }

    private void Start()
    {
        UnityBattleController.Instance.StateChanged += StateChanged;
    }

    private void StateChanged(UnityBattleController.States oldState, UnityBattleController.States newState)
    {
        if (newState != UnityBattleController.States.InitBattle) return;
        //This will later handle things like scene transitions and initial camera movement
        UnityBattleController.Instance.SetState(UnityBattleController.States.MainMenu);
    }
}
