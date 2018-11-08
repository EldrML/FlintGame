using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActionButton : MonoBehaviour
{

    [SerializeField]
    private Text _name;

    [SerializeField]
    private ActionData _action;

    public ActionData Action { get { return _action; } set { _action = value; _name.text = _action.Name; } }

    public void ExecuteAction()
    {
        UnityBattleController.Instance.AddActionForCurrentCharacter(_action);
    }

}
