using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleMenu : MonoSingleton<UIBattleMenu>
{

    [SerializeField]
    private RectTransform _enemyParent;
    [SerializeField]
    private RectTransform _partyParent;
    [SerializeField]
    private RectTransform _mainMenuParent;
    [SerializeField]
    private RectTransform _actionTypeMenu;
    [SerializeField]
    private RectTransform _actionMenuParent;
    [SerializeField]
    private RectTransform _actionMenuButtonParent;
    [SerializeField]
    private RectTransform _selectTargetMenu;

    [SerializeField]
    private UIActionButton _actionButtonPrefab;
    [SerializeField]
    private UIEntity _entityButtonPrefab;

    protected override UIBattleMenu GetSingletonInstance { get { return this; } }

    private void Start()
    {
        UnityBattleController.Instance.StateChanged += StateChanged;
        HideAllMenus();
    }

    private void StateChanged(UnityBattleController.States oldState, UnityBattleController.States newState)
    {
        switch (newState)
        {
            case UnityBattleController.States.InitBattle:
                InitEnemies();
                InitParty();
                _partyParent.gameObject.SetActive(true);
                _enemyParent.gameObject.SetActive(true);
                break;
            case UnityBattleController.States.MainMenu:
                if (oldState != UnityBattleController.States.InitBattle) InitEnemies();
                ShowMainMenu();
                break;
            case UnityBattleController.States.SelectAction:
                ShowActionTypeMenu();
                break;
            case UnityBattleController.States.SelectTargets:
                HideAllMenus();
                break;
            case UnityBattleController.States.EndBattle:
                HideAllMenus();
                _partyParent.gameObject.SetActive(false);
                _enemyParent.gameObject.SetActive(false);
                break;
        }
    }

    private void InitEnemies()
    {
        for (int i = _enemyParent.childCount - 1; i >= 0; i--)
            Destroy(_enemyParent.GetChild(i).gameObject);

        foreach (var enemy in UnityBattleController.Instance.Enemies)
        {
            var obj = Instantiate(_entityButtonPrefab, _enemyParent);

            obj.Entity = enemy;
        }
    }

    private void InitParty()
    {
        for (int i = _partyParent.childCount - 1; i >= 0; i--)
            Destroy(_partyParent.GetChild(i).gameObject);

        foreach (var member in PartyController.Instance.PartyMembers)
        {
            var obj = Instantiate(_entityButtonPrefab, _partyParent);

            obj.Entity = member;
        }
    }

    private void HideAllMenus()
    {
        _mainMenuParent.gameObject.SetActive(false);
        _actionMenuParent.gameObject.SetActive(false);
        _selectTargetMenu.gameObject.SetActive(false);
        _actionTypeMenu.gameObject.SetActive(false);
    }

    private void ShowMainMenu()
    {
        HideAllMenus();
        _mainMenuParent.gameObject.SetActive(true);
    }

    public void ShowActionTypeMenu()
    {
        HideAllMenus();
        _actionTypeMenu.gameObject.SetActive(true);
    }

    public void ShowActionMenu(int actionType)
    {
        HideAllMenus();
        _actionMenuParent.gameObject.SetActive(true);
        for (int i = _actionMenuButtonParent.childCount - 1; i >= 0; i--)
            Destroy(_actionMenuButtonParent.GetChild(i).gameObject);

        var actions = UnityBattleController.Instance.CurrentCharacterActions((ActionData.ActionType)actionType);

        foreach (var action in actions)
        {
            var obj = Instantiate(_actionButtonPrefab, _actionMenuButtonParent);

            obj.Action = action;
        }
    }

    private void ShowTargeting()
    {
        HideAllMenus();
        _selectTargetMenu.gameObject.SetActive(true);
    }

}
