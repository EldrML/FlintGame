using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    //An ItemStack is an object that represents a stack of items on the inventory.
    // It's meant to be used by the character inventory for adding and removing items from stacks.

    public Item itemType = null;
    public int numberOfItems = 0;

    public bool Empty
    {
        get
        {
            return numberOfItems <= 0;
        }
    }

    public ItemStack(Item itemType, int numberOfItems)
    {
        this.itemType = itemType;
        this.numberOfItems = Mathf.Clamp(numberOfItems, 1, itemType.limit);
    }

    public int addToStack(int number)
    {
        // Returns possible overflow
        int overflow = 0;

        this.numberOfItems = this.numberOfItems + number;

        if (this.numberOfItems > this.itemType.limit)
        {

            overflow = this.numberOfItems - this.itemType.limit;
            this.numberOfItems = this.itemType.limit;
        }

        return overflow;
    }

    public int removeFromStack(int number)
    {
        // Returns possible underflow
        int underflow = 0;

        this.numberOfItems = this.numberOfItems - number;

        if (this.numberOfItems < 0)
        {
            underflow = Mathf.Abs(this.numberOfItems);
            this.numberOfItems = 0;
        }

        return underflow;
    }

}
