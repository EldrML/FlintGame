using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateFightMenu : BaseState {

    public const string MENU_FIGHT = "fight";
    public const string MENU_PSY = "psy";
    public const string MENU_DJINN = "djinn";
    public const string MENU_SUMMON = "summon";
    public const string MENU_ITEM = "item";
    public const string MENU_DEFEND = "defend";
    public List<string> menuOption = new List<string>();
    public int selectedMenuOption = 0;

    public void SetUp(BattleController controller)
    {
        var ch = controller.GetCurrentCharacter();
        menuOption = new List<string>();
        if (ch.CanAttack())
        {
            menuOption.Add(MENU_FIGHT);
        }
        if (ch.GetPsy().Count>0)
        {
            menuOption.Add(MENU_PSY);
        }
        if (ch.GetDjinn().Count > 0)
        {
            menuOption.Add(MENU_DJINN);
        }
        if (controller.GetCharacterSummons().Count > 0)
        {
            menuOption.Add(MENU_SUMMON);
        }
        if (ch.GetItems().Count > 0)
        {
            menuOption.Add(MENU_ITEM);
        }
        //ensure that at least "defend" is an option
        menuOption.Add(MENU_DEFEND);
    }

    //return true if there is a transition, false otherwise
    public override void CheckForStateAndChange(BattleController controller, string action)
    {
        if (action == BattleController.ACTION_AGREE)
        {
            switch (menuOption[selectedMenuOption])
            {
                case MENU_FIGHT:
                    controller.sSelectTarget.SetUp(controller.GetCurrentCharacter(),controller.GetCurrentCharacter().Weapon, controller);
                    controller.CurrentState = controller.sSelectTarget;
                    break;
                case MENU_PSY:
                    controller.sPsyMenu.SetUp(controller.GetCurrentCharacter());
                    controller.CurrentState = controller.sPsyMenu;
                    break;
                case MENU_DJINN:
                    controller.sDjinnMenu.SetUp(controller.GetCurrentCharacter());
                    controller.CurrentState = controller.sDjinnMenu;
                    break;
                case MENU_SUMMON:
                    controller.sSummonMenu.SetUp(controller);
                    controller.CurrentState = controller.sSummonMenu;
                    break;
                case MENU_ITEM:
                    controller.sItemMenu.SetUp(controller.GetCurrentCharacter());
                    controller.CurrentState = controller.sItemMenu;
                    break;
                case MENU_DEFEND:
                    controller.GetCurrentCharacter().ChosenSkill = Defend.DEFEND;
                    controller.CurrentState = controller.sFinaliseCharacter;
                    break;
                default:
                    break;
            }
        }
        if (action == BattleController.ACTION_BACK)
        {
            //if it's the first character, back out to the fight/escape menu
            if (controller.GetCharacters().First() == controller.GetCurrentCharacter())
            {
                controller.CurrentState = controller.sMainMenu;
            }
            else
            {
                //if it's anyone else, back out to the previous character.
                controller.SelectPreviousCharacter();
                selectedMenuOption = 0;
                SetUp(controller);
            }
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
