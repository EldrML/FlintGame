using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateRenderBattle : BaseState
{

    public List<Character> RenderTargets {get;set;}
    public void ProgressFrame(int frames){
        int frameCount = _renderFrames.First();
        frameCount -= frames;
        if(frameCount<0){
            frameCount = 0;
        }
        _renderFrames[0] = frameCount;
    }
    //TODO: implement any metadata for animations here
    public List<int> _renderFrames = new List<int>();
    private bool WaitingForAction = true;

    public bool IsCurrentAnimationComplete()
    {
        if(RenderTargets.Count==0){
            return true;//there are no characters, shouldn't get here
        }
        if(_renderFrames.First()>0){
            return false;//animation is running
        }
        if(WaitingForAction){
            return false;//text is displaying
        }
        return true;//everything is done
    }

    public void SetUp(BattleController controller, List<Character> characters)
    {
        WaitingForAction = true;
        RenderTargets = characters;
        foreach(var ch in characters){
            _renderFrames.Add(50);//TODO: replace with real frame data
        }
    }

    //return true if there is a transition, false otherwise
    public override void CheckForStateAndChange(BattleController controller, string action)
    {
        if (RenderTargets.Count == 0)
        {
            //TODO: change state when all animations are done
            controller.sMainMenu.selectedMenuOption = StateMainMenu.MENU_OPTION_FIGHT;
            controller.CurrentState = controller.sMainMenu;
        }
    }

    //handles game logic for arrow keys which cannot alter the states
    //also handles cases where a/b do not change state
    public override void Logic(string action)
    {
        if(action == BattleController.ACTION_AGREE){
            //TODO: progress text
            WaitingForAction = false;
        }

        if (action == BattleController.ACTION_BACK)
        {
            if (_renderFrames[0] > 0)
            {
                _renderFrames[0] = 0;//skip the current animation
            }
            else
            {
                //progress text
                WaitingForAction = false;
            }
        }
        if(IsCurrentAnimationComplete()){
            WaitingForAction = true;
            _renderFrames.RemoveAt(0);
            RenderTargets.RemoveAt(0);
        }
    }

}
