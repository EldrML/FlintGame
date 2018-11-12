using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogController : MonoSingleton<UIDialogController>
{

    protected override UIDialogController GetSingletonInstance { get { return this; } }
    private Text _dialogText;
    private GameObject _panel;
    private GameObject _actor;

    protected override void Awake()
    {
        base.Awake();
        _panel = this.gameObject;
        _dialogText = _panel.GetComponentInChildren<Text>();
        _panel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Accept/Use"))
        {
            this.EndDialog();
        }
    }

    public void StartDialog(GameObject actor, string text)
    {
        _panel.SetActive(true);
        _actor = actor;
        _dialogText.text = text;

        _actor.GetComponent<MainCharacter>().Disable();
    }

    public void ContinueDialog()
    {

    }

    public void EndDialog()
    {
        _panel.SetActive(false);
        _actor.GetComponent<MainCharacter>().Enable();
    }
}
