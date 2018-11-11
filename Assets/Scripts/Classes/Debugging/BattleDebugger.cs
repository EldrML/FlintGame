using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDebugger : MonoBehaviour
{

    [SerializeField]
    private BattleSettings _settings;

    private void Start()
    {
        UnityBattleController.Instance.StateChanged += StateChanged;
        GameController.Instance.GameStarted += () => gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void StateChanged(UnityBattleController.States oldState, UnityBattleController.States newState)
    {
        if (newState != UnityBattleController.States.EndBattle) return;

        gameObject.SetActive(true);
    }

    public void StartDebugBattle()
    {
        UnityBattleController.Instance.StartBattle(_settings);
    }



}
