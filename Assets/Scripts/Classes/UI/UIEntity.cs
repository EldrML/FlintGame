using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEntity : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    private EntityData _entity;

    private void Start()
    {
        UnityBattleController.Instance.ActingPartyMemberChanged += UpdateText;
    }

    public EntityData Entity { get { return _entity; }
        set
        {
            if (_entity) UnsubscribeEvents();
            _entity = value;
            SubscribeEvents();
            UpdateText();
        }
    }

    private void SubscribeEvents()
    {
        UnsubscribeEvents();
        _entity.HealthChanged += UpdateText;
        _entity.ActingStateChanged += UpdateText;
    }

    private void UnsubscribeEvents()
    {
        if (!_entity) return;
        _entity.HealthChanged -= UpdateText;
        _entity.ActingStateChanged -= UpdateText;
    }

    public void SelectAsTarget()
    {
        UnityBattleController.Instance.AddTargetToCurrentAction(_entity);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void UpdateText()
    {
        if (!_entity || !_text) return;
        if (_entity.IsActing) _text.text = "[" + _entity.Name + "[" + _entity.HealthPoints + "]]";
        else if (UnityBattleController.Instance.CurrentActingPartyMember == _entity) _text.text = "-> " + _entity.Name + "[" + _entity.HealthPoints + "] <-";
        else _text.text = _entity.Name + "[" + _entity.HealthPoints + "]";
    }
}
