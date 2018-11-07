using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateItemMenu : BaseState {

    public List<string> menuOption = new List<string>();
    public int selectedMenuOption = 0;

    public void SetUp(Character ch)
    {
        selectedMenuOption = 0;
        List<string> Items = ch.GetItems();
        menuOption = new List<string>();
        menuOption.AddRange(Items);
    }

    //return true if there is a transition, false otherwise
    public override void CheckForStateAndChange(BattleController controller, string action)
    {
        //TODO: "A" button. Items not implemented for now, behaviour will be similar to other skills but are consumable.
        if (action == BattleController.ACTION_BACK)
        {
            controller.CurrentState = controller.sFightMenu;
        }
    }

    //handles game logic for arrow keys which cannot alter the states
    //also handles cases where a/b do not change state
    public override void Logic(string action)
    {
        //menu scrolling
        if (action == BattleController.ACTION_DOWN)
        {
            selectedMenuOption += 1;
            if (selectedMenuOption >= menuOption.Count)
            {
                selectedMenuOption = 0;
            }
        }
        if (action == BattleController.ACTION_UP)
        {
            selectedMenuOption -= 1;
            if (selectedMenuOption < 0)
            {
                selectedMenuOption = menuOption.Count - 1;
            }
        }
    }
}
