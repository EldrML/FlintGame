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
            Debug.Log(actor.name + " receives a " + objectToGet.objectName);
            actor.GetComponent<CharacterInventory>().addItem(objectToGet, 1);
            objectToGet = null;
        }
    }
}
