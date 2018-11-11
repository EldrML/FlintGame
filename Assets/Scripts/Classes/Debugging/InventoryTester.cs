using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTester : MonoBehaviour
{
    public InputField itemNumber;
    public CharacterInventory characterInventory;

    public Item itemToAdd;

    public void addHerbs()
    {
        int quantity = int.Parse(itemNumber.text);
        Debug.Log("Adding " + quantity + " herbs to Isaac's inventory");
        characterInventory.addItem(itemToAdd, quantity);
    }

    public void removeHerbs()
    {
        int quantity = int.Parse(itemNumber.text);
        Debug.Log("Removing " + quantity + " herbs from Isaac's inventory");
        characterInventory.removeItem(itemToAdd, quantity);
    }
}
