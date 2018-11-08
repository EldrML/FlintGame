using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateFinaliseCharacter : BaseState {

    //return true if there is a transition, false otherwise
    public override void CheckForStateAndChange(BattleController controller, string action)
    {
        //if there are more characters to select, progress to the next character, otherwise proceed to the combat animation
        if (controller.GetCharacters().Last() == controller.GetCurrentCharacter())
        {
            if (controller.EnemyActionsReady())
            {
                //TODO: implement render.SetUp() if needed
                controller.SelectFirstCharacter();
                //commit battle actions for the turn
                var renderTargets = CommitBattleActions(controller);

                controller.sRenderBattle.SetUp(controller, renderTargets);
                controller.CurrentState = controller.sRenderBattle;
            }
        }
        else
        {
            controller.SelectNextCharacter();
            controller.sFightMenu.selectedMenuOption = 0;//re-select 'fight' by default between turns
            controller.CurrentState = controller.sFightMenu;
        }
    }

    //handles game logic for arrow keys which cannot alter the states
    //also handles cases where a/b do not change state
    public override void Logic(string action)
    {
    }

    private List<Character> CommitBattleActions(BattleController controller)
    {

        List<Character> renderTargets = new List<Character>();

        List<Character> characters = controller.GetCharacters();
        List<Character> enemies = controller.GetEnemies();
        var allCharacters = new List<Character>();
        allCharacters.AddRange(characters);
        allCharacters.AddRange(enemies);

        allCharacters = allCharacters.OrderBy( x=>x.ChosenSkill.AttackType != AttackType.EFFECT_MOVE_FIRST).ToList();
        //TODO: things like: .ThenBy(x=>x.agility) or however turn order is determined

        //TODO: all other things, e.g. AI/turn order, reset djinn, turn based efects, etc required for battles...
        foreach (Character character in allCharacters)
        {
            //targeting own team
            var targetTeam = enemies;
            if (character.ChosenSkill.Target == TargetType.ALLY){
                targetTeam = characters;
            }
            Character target = targetTeam.Where(x => x.Name == character.ChosenTarget).FirstOrDefault();
            //if the target is gone, choose a default new target. TODO: maybe choose an enemy near the old target?
            if(target==null && enemies.Count() > 0)
            {
                target = enemies.FirstOrDefault();
            }
            if (target != null)
            {
                if (character.ChosenSkill is Weapon)
                {
                    //do basic attack & check for unleash
                    //TODO: for unleashes need to check target in case they hit ally (treat ally tgt as caster)
                }
                if (character.ChosenSkill == Defend.DEFEND)
                {
                    //do defend
                }
                if(character.ChosenSkill is Psy)
                {
                    character.pp -= ((Psy)character.ChosenSkill).Cost;
                }
                if (character.ChosenSkill is Djinn)
                {
                    Djinn chosenDjinn = ((Djinn)character.ChosenSkill);
                    if (chosenDjinn.State == Djinn.DjinnState.STATE_STANDBY) {
                        chosenDjinn.State = Djinn.DjinnState.STATE_SET;
                    }
                    if (chosenDjinn.State == Djinn.DjinnState.STATE_SET)
                    {
                        chosenDjinn.State = Djinn.DjinnState.STATE_STANDBY;
                    }
                }
                if (character.ChosenSkill is Summon)
                {
                    Summon chosenSummon = ((Summon)character.ChosenSkill);
                    List<Djinn> cost = new List<Djinn>();
                    //TODO: refactor when fixing summons
                    //For testing, using hard-coded summons.
                    //pull out the djinn used for summoning, and set them to be used up
                    if ( chosenSummon.Name == "Summon earth 1")
                    {
                        cost.AddRange(character.GetDjinn().Where(x => x.State == Djinn.DjinnState.STATE_STANDBY && x.Element == Djinn.DjinnElement.ELEMENT_FIRE));
                    }
                    if (chosenSummon.Name == "Summon fire 1")
                    {
                        cost.AddRange(character.GetDjinn().Where(x => x.State == Djinn.DjinnState.STATE_STANDBY && x.Element == Djinn.DjinnElement.ELEMENT_FIRE).Take(1));
                    }
                    if (chosenSummon.Name == "Summon fire 2")
                    {
                        cost.AddRange(character.GetDjinn().Where(x => x.State == Djinn.DjinnState.STATE_STANDBY && x.Element == Djinn.DjinnElement.ELEMENT_FIRE).Take(2));
                    }
                    if (chosenSummon.Name == "Summon Fire2Earth1")
                    {
                        cost.AddRange(character.GetDjinn().Where(x => x.State == Djinn.DjinnState.STATE_STANDBY && x.Element == Djinn.DjinnElement.ELEMENT_FIRE).Take(2));
                        cost.AddRange(character.GetDjinn().Where(x => x.State == Djinn.DjinnState.STATE_STANDBY && x.Element == Djinn.DjinnElement.ELEMENT_FIRE).Take(1));
                    }
                    Dictionary<Djinn.DjinnElement, int> cooldowns = new Dictionary<Djinn.DjinnElement, int>();
                    cooldowns.Add(Djinn.DjinnElement.ELEMENT_EARTH, cost.Where(x => x.Element == Djinn.DjinnElement.ELEMENT_EARTH).Count());
                    cooldowns.Add(Djinn.DjinnElement.ELEMENT_FIRE, cost.Where(x => x.Element == Djinn.DjinnElement.ELEMENT_FIRE).Count());
                    cooldowns.Add(Djinn.DjinnElement.ELEMENT_AIR, cost.Where(x => x.Element == Djinn.DjinnElement.ELEMENT_AIR).Count());
                    cooldowns.Add(Djinn.DjinnElement.ELEMENT_WATER, cost.Where(x => x.Element == Djinn.DjinnElement.ELEMENT_WATER).Count());
                    foreach (Djinn djinn in cost)
                    {
                        djinn.State = Djinn.DjinnState.STATE_SUMMON;
                        djinn.Cooldown = cooldowns[djinn.Element];
                        cooldowns[djinn.Element] -= 1;
                    }
                }
            }
            renderTargets.Add(character);
        }
        return renderTargets;
    }

}
