using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainerInteractable : Interactable
{
    public Item objectToGet;

    public override void Interact(GameObject actor)
    {
        base.Interact(actor);
        if (objectToGet != null)
        {
            UIDialogController.Instance.StartDialog(actor.name + " received: " + objectToGet.objectName);
            actor.GetComponent<CharacterInventory>().addItem(objectToGet, 1);
            objectToGet = null;
        }
    }
}
