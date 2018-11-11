using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(GameObject actor)
    {
        Debug.Log(actor.name + " interacted with this " + this.name);
    }
}
