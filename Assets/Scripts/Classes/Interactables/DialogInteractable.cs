using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractable : Interactable
{
    public string text;

    public override void Interact(GameObject actor)
    {
        base.Interact(actor);
        if (text != null)
        {
            Debug.Log(actor.name + " is reading this " + this.name + ". It says: " + text);
            UIDialogController.Instance.StartDialog(actor, text);
        }
    }
}
