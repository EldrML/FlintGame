﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateSummonMenu : BaseState {

    public List<string> menuOption = new List<string>();
    public int selectedMenuOption = 0;

    public void SetUp(BattleController b)
    {
        selectedMenuOption = 0;
        List<string> Summon = b.GetCharacterSummons().Select(x=>x.Name).ToList();
        menuOption = new List<string>();
        menuOption.AddRange(Summon);
    }

    //return true if there is a transition, false otherwise
    public override void CheckForStateAndChange(BattleController controller, string action)
    {
        if(action == BattleController.ACTION_AGREE)
        {
            string skillName = menuOption[selectedMenuOption];
            var summon = controller.GetCharacterSummons().Where(d => d.Name == skillName).FirstOrDefault();
            controller.sSelectTarget.SetUp(controller.GetCurrentCharacter(), summon, controller);
            controller.CurrentState = controller.sSelectTarget;
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
