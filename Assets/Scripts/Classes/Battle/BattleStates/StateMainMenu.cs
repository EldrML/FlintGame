using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMainMenu : BaseState {

    public const string MENU_OPTION_FIGHT = "Fight";
    public const string MENU_OPTION_FLEE = "Flee";
    //TODO: "other" menu if needed

    public string selectedMenuOption = MENU_OPTION_FIGHT;
    
    //return true if there is a transition, false otherwise
    public override void CheckForStateAndChange(BattleController controller, string action)
    {
        if(action == BattleController.ACTION_AGREE)
        {
            if(selectedMenuOption == MENU_OPTION_FIGHT)
            {
                controller.sFightMenu.SetUp(controller);
                controller.sFightMenu.selectedMenuOption = 0;
                controller.CurrentState = controller.sFightMenu;
                //TODO: init fight menu with character options (e.g. djinn available, summons available, etc)
            }
            if (selectedMenuOption == MENU_OPTION_FLEE)
            {
                //TODO: actual flee logic
            }
        }
    }

    //handles game logic for arrow keys which cannot alter the states
    //also handles cases where a/b do not change state
    public override void Logic(string action)
    {
        //menu scrolling
        if (action == BattleController.ACTION_LEFT ||
            action == BattleController.ACTION_RIGHT)
        {
            if (selectedMenuOption == MENU_OPTION_FIGHT)
            {
                selectedMenuOption = MENU_OPTION_FLEE;
            }
            else//only have 2 options at the moment, can just toggle between them
            {
                selectedMenuOption = MENU_OPTION_FIGHT;
            }
        }
    }
    
}
