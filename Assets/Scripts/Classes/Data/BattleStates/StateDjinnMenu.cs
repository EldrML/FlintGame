using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateDjinnMenu : BaseState {
    
    public List<string> menuOption = new List<string>();
    public int selectedMenuOption = 0;

    public void SetUp(Character ch)
    {
        selectedMenuOption = 0;
        List<string> Djinn = ch.GetDjinn().Select(x=>x.Name).ToList();
        menuOption = new List<string>();
        menuOption.AddRange(Djinn);
    }


    //return true if there is a transition, false otherwise
    public override void CheckForStateAndChange(BattleController controller, string action)
    {
        if(action == BattleController.ACTION_AGREE)
        {
            string skillName = menuOption[selectedMenuOption];
            var djinn = controller.GetCurrentCharacter().GetDjinn().Where(d => d.Name == skillName).FirstOrDefault();
            if (djinn.State == Djinn.DjinnState.STATE_SET)
            {
                controller.sSelectTarget.SetUp(controller.GetCurrentCharacter(), djinn, controller);
                controller.CurrentState = controller.sSelectTarget;
            }
            if(djinn.State == Djinn.DjinnState.STATE_STANDBY)
            {
                controller.sSelectTarget.SetUp(controller.GetCurrentCharacter(), djinn, controller);
                controller.CurrentState = controller.sSelectTarget;
            }
            //else summon, djinn can't be changed
        }
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
