using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateSelectTarget : BaseState {

    public bool targetsEnemy = true;
    public List<string> menuOption = new List<string>();
    public int selectedMenuOption = 0;

    public void SetUp (Character ch, Skill skill, BattleController controller)
    {
        TargetType targetType = skill.Target;
        targetsEnemy = true;
		if(targetType==TargetType.ALLY){
			targetsEnemy = false;
		}
        ch.ChosenSkill = skill;
        menuOption = new List<string>();
        if (targetsEnemy)
        {
            menuOption.AddRange(controller.GetEnemies().Select(x => x.Name));
        }
        else
        {
            menuOption.AddRange(controller.GetCharacters().Select(x => x.Name));
        }
    }

    //return true if there is a transition, false otherwise
    public override void CheckForStateAndChange(BattleController controller, string action)
    {
        if(action == BattleController.ACTION_AGREE)
        {
            Character ch = controller.GetCurrentCharacter();
            if (targetsEnemy)
            {
                ch.ChosenTarget = controller.GetEnemies().ElementAt(selectedMenuOption).Name;
            }
            else
            {
                ch.ChosenTarget = controller.GetCharacters().ElementAt(selectedMenuOption).Name;
            }

            controller.CurrentState = controller.sFinaliseCharacter;
        }
        if (action == BattleController.ACTION_BACK)
        {
            Character ch = controller.GetCurrentCharacter();
            ch.ChosenSkill = Defend.DEFEND;
            controller.CurrentState = controller.sFightMenu;
        }
    }

    //handles game logic for arrow keys which cannot alter the states
    //also handles cases where a/b do not change state
    public override void Logic(string action)
    {
        //menu scrolling
        if (action == BattleController.ACTION_RIGHT)
        {
            selectedMenuOption += 1;
            if (selectedMenuOption >= menuOption.Count)
            {
                selectedMenuOption = 0;
            }
        }
        if (action == BattleController.ACTION_LEFT)
        {
            selectedMenuOption -= 1;
            if (selectedMenuOption < 0)
            {
                selectedMenuOption = menuOption.Count - 1;
            }
        }
    }

}
