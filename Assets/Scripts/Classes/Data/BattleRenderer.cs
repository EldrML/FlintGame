using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;

public class BattleRenderer  {
    static MethodInfo _clearConsoleMethod;
    static MethodInfo clearConsoleMethod
    {
        get
        {
            if (_clearConsoleMethod == null)
            {
                Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
                Type logEntries = assembly.GetType("UnityEditor.LogEntries");
                _clearConsoleMethod = logEntries.GetMethod("Clear");
            }
            return _clearConsoleMethod;
        }
    }

    public static void ClearLogConsole()
    {
        clearConsoleMethod.Invoke(new object(), null);
    }
    public void Render(BattleController controller)
    {
  
        Debug.ClearDeveloperConsole();
        ClearLogConsole();
        if (controller.CurrentState == controller.sDjinnMenu)
        {
            Debug.Log("State: Djinn Menu");
            string debugLog = "";
            int i = 0;
            var djinn = controller.GetCurrentCharacter().GetDjinn();
            foreach(var item in controller.sDjinnMenu.menuOption)
            {
                if (i > 0)
                {
                    debugLog += "\n";
                }
                if (i== controller.sDjinnMenu.selectedMenuOption)
                {
                    debugLog += "-";
                }
                var optDjinn = djinn.Where(x => x.Name == item).FirstOrDefault();
                if(optDjinn != null)
                {
                    debugLog += optDjinn.Element;
                    debugLog += "(" + optDjinn.State +" "+Convert.ToString(optDjinn.Cooldown) + ")";
                }
                debugLog += item;
                i += 1;
            }
            Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sFightMenu)
        {
            Debug.Log("State: Fight Menu");
            string debugLog = "";
            debugLog += controller.GetCurrentCharacter().Name + ":";
            int i = 0;
            foreach (var item in controller.sFightMenu.menuOption)
            {
                if (i > 0)
                {
                    debugLog += ",";
                }
                if (i == controller.sFightMenu.selectedMenuOption)
                {
                    debugLog += "-";
                }
                debugLog += item;
                i += 1;
            }
            Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sItemMenu)
        {
            Debug.Log("State: Item Menu");
            string debugLog = "";
            int i = 0;
            foreach (var item in controller.sItemMenu.menuOption)
            {
                if (i > 0)
                {
                    debugLog += "\n";
                }
                if (i == controller.sItemMenu.selectedMenuOption)
                {
                    debugLog += "-";
                }
                debugLog += item;
                i += 1;
            }
            Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sSummonMenu)
        {
            Debug.Log("State: Summon Menu");
            string debugLog = "";
            int i = 0;
            foreach (var item in controller.sSummonMenu.menuOption)
            {
                if (i > 0)
                {
                    debugLog += "\n";
                }
                if (i == controller.sSummonMenu.selectedMenuOption)
                {
                    debugLog += "-";
                }
                debugLog += item;
                i += 1;
            }
            Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sMainMenu)
        {
            Debug.Log("State: Main Menu");
            string debugLog = "";
            if(controller.sMainMenu.selectedMenuOption == StateMainMenu.MENU_OPTION_FIGHT)
            {
                debugLog += "-";
            }
            debugLog += StateMainMenu.MENU_OPTION_FIGHT;
            debugLog += ",";
            if (controller.sMainMenu.selectedMenuOption == StateMainMenu.MENU_OPTION_FLEE)
            {
                debugLog += "-";
            }
            debugLog += StateMainMenu.MENU_OPTION_FLEE;
            Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sPsyMenu)
        {
            Debug.Log("State: Psy Menu");
            string debugLog = "";
            int i = 0;
            foreach (var item in controller.sPsyMenu.menuOption)
            {
                if (i > 0)
                {
                    debugLog += "\n";
                }
                if (i == controller.sPsyMenu.selectedMenuOption)
                {
                    debugLog += "-";
                }
                debugLog += item;
                i += 1;
            }
            Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sSelectTarget)
        {
            Debug.Log("State: Select Target");
            string debugLog = "";
            int i = 0;
            var optSkill = controller.GetCurrentCharacter().ChosenSkill;
            foreach (var item in controller.sSelectTarget.menuOption)
            {
                if (i > 0)
                {
                    debugLog += ",";
                }
                if (i == controller.sSelectTarget.selectedMenuOption || 
                    optSkill.Range == RangeType.ALL)
                {
                    debugLog += "-";
                }
                if (optSkill.Range == RangeType.FIVE)
                {
                    if (i != controller.sSelectTarget.selectedMenuOption &&
                        i >= controller.sSelectTarget.selectedMenuOption - 2 &&
                        i <= controller.sSelectTarget.selectedMenuOption + 2)
                    {
                        debugLog += "*";
                    }
                }
                if (optSkill.Range == RangeType.THREE)
                {
                    if (i != controller.sSelectTarget.selectedMenuOption &&
                        i >= controller.sSelectTarget.selectedMenuOption - 1 &&
                        i <= controller.sSelectTarget.selectedMenuOption + 1)
                    {
                        debugLog += "*";
                    }
                }
                debugLog += item;
                i += 1;
            }
            Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sFinaliseCharacter)
        {
            //shouldn't get here (or at most for 1 frame)
            //in multiplayer (not yet impleneted) this can persist for some time, 
            //so a message needs to be shown that we are waiting on the opponent
            Debug.Log("State: Select Finalise Character");
            Debug.Log("State: Waiting on enemy input...");
        }
        if(controller.CurrentState == controller.sRenderBattle)
        {
            Debug.Log("State:  Render Battle");
            string debugLog = "";
            var characters= controller.sRenderBattle.RenderTargets;
            if(characters.Count>0){
                var ch = characters.First();
                if (ch.ChosenSkill != Defend.DEFEND)
                {
                    debugLog += ch.Name + " uses: " + ch.ChosenSkill.Name + ", at: " + ch.ChosenTarget;
                }
                else
                {
                    debugLog += ch.Name + " defends.";
                }
            }
            Debug.Log(debugLog);
            controller.sRenderBattle.ProgressFrame(1);
        }

    }
}
