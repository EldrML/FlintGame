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
        BattleController.Instance.StateChanged += StateChanged;
        GameController.Instance.GameStarted += () => gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void StateChanged(BattleController.States oldState, BattleController.States newState)
    {
        if (newState != BattleController.States.EndBattle) return;

        gameObject.SetActive(true);
    }

    public void StartDebugBattle()
    {
        BattleController.Instance.StartBattle(_settings);
    }



}
