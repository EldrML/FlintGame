using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogController : MonoSingleton<UIDialogController>
{

    protected override UIDialogController GetSingletonInstance { get { return this; } }
    private Text _dialogText;
    private GameObject _panel;

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

    public void StartDialog(string text)
    {
        _panel.SetActive(true);
        _dialogText.text = text;

        MainCharacterController.Instance.Disable();
    }

    public void ContinueDialog()
    {

    }

    public void EndDialog()
    {
        _panel.SetActive(false);
        MainCharacterController.Instance.Enable();
    }
}
