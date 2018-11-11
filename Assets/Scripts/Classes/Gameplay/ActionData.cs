using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionData : ScriptableObject, IHasName, IHasDescription
{
    public enum ActionType { Psy, Djinn, Summon, Inventory }
    public enum ActionElement { None, Fire, Water, Earth, Air }
    public enum ActionRange { One, Three, All }

    [SerializeField]
    private string _name = "NameHere";
    [SerializeField]
    private string _description = "DescriptionHere";
    [SerializeField]
    private ActionElement _element = ActionElement.None;
    [SerializeField]
    private ActionRange _range = ActionRange.One;
    [SerializeField]
    private int _power = 1;

    public int TargetCount = 1;

    public string Name { get { return _name; } }
    public string Description { get { return _description; } }
    public ActionElement Element { get { return _element; } }
    public ActionRange Range { get { return _range; } }
    public int Power { get { return _power; } }

    public IEnumerator ExecuteAction(EntityData instigator, IEnumerable<EntityData> targets)
    {
        instigator.IsActing = true;

        yield return UnityBattleController.Instance.StartCoroutine(ExecuteActionInternal(instigator, targets));

        instigator.IsActing = false;
    }

    protected abstract IEnumerator ExecuteActionInternal(EntityData instigator, IEnumerable<EntityData> targets);
}
