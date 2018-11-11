using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInventory : MonoBehaviour
{
    // Represents a character inventory.
    public List<ItemStack> inventoryItems;

    // Adds items. Agnostic to stacks.
    public void addItem(Item item, int quantity)
    {
        ItemStack usableStack = this.findAddableStack(item);
        if (usableStack != null)
        {
            int overflow = usableStack.addToStack(quantity);
            if (overflow > 0)
            {
                this.addItem(item, overflow);
            }
        }
        else
        {
            inventoryItems.Add(new ItemStack(item, 1));
            this.addItem(item, quantity - 1);
        }
    }

    // Removes items. Agnostic to stacks.
    public void removeItem(Item item, int quantity)
    {
        ItemStack usableStack = this.findRemovableStack(item);
        if (usableStack != null)
        {
            int underflow = usableStack.removeFromStack(quantity);
            this.cleanEmptyStacks();
            if (underflow > 0)
            {
                this.removeItem(item, underflow);
            }
        }
        else
        {
            Debug.LogWarning("Trying to remove items from an inventory with none of that type!");
        }

    }

    // This is needed to ensure no 0 item stacks are being used.
    private void cleanEmptyStacks()
    {
        inventoryItems.RemoveAll(stack => stack.numberOfItems <= 0);
    }

    // Returns the appropriate stack to add items to
    // Returns null if there is not any available.
    private ItemStack findAddableStack(Item item)
    {
        // Returns null if no stack is found.
        foreach (ItemStack stack in this.inventoryItems)
        {
            if (stack.itemType.GetType() == item.GetType())
            {
                if (stack.numberOfItems < stack.itemType.limit)
                {
                    return stack;
                }
            }
        }
        return null;
    }

    private ItemStack findRemovableStack(Item item)
    {
        //Returns null if no stack is found.
        List<ItemStack> possibleStacks = new List<ItemStack>();
        foreach (ItemStack stack in this.inventoryItems)
        {
            if (stack.itemType.GetType() == item.GetType())
            {
                possibleStacks.Add(stack);
            }
        }

        if (possibleStacks.Count == 0)
        {
            return null;
        }

        ItemStack lowestStack = possibleStacks[0];
        foreach (ItemStack stack in possibleStacks)
        {
            if (stack.numberOfItems < lowestStack.numberOfItems)
            {
                lowestStack = stack;
            }
        }
        return lowestStack;
    }
}
