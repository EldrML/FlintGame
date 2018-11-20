using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using UnityEngine.UI;

public class BattleRenderer : MonoBehaviour
{

    private float rotation = 0;
    private float center_x = 0;
    private float center_y = 0;

    public Text TextElement { get; set; }
    public Image FightMenuImage { get; set; }
    public Image FleeMenuImage { get; set; }
    public Image DjinnMenuImage { get; set; }
    public Image PsyMenuImage { get; set; }
    public Image SummonMenuImage { get; set; }
    public Image ItemMenuImage { get; set; }
    public Image AttackMenuImage { get; set; }
    public Image DefendMenuImage { get; set; }

    //Note this is just for testing, to be replaced with proper images later
    private Dictionary<string, GameObject> chImages = new Dictionary<string, GameObject>();

    [System.Serializable]
    public enum ANIMATION_STATE
    {
        A_IDLE = 0,
        A_IDLE_REVERSE = 1,
        A_ATTACK = 2,
        A_ATTACK_REVERSE = 3,
        A_HIT = 4,
        A_HIT_REVERSE = 5,
        A_SUMMON = 6,
        A_SUMMON_REVERSE = 7
    }

    private float orbitHeight = 3f;
    private float characterXOffset = 1f;
    private float spriteScaleFactor = 2f;

    private GameObject sprite;

    private void Start()
    {
        TextElement = GameObject.Find("BattleTextOutput").GetComponent<Text>();
        FightMenuImage = GameObject.Find("UIFight").GetComponent<Image>();
        FleeMenuImage = GameObject.Find("UIFlee").GetComponent<Image>();
        DjinnMenuImage = GameObject.Find("UIDjinn").GetComponent<Image>();
        PsyMenuImage = GameObject.Find("UIPsy").GetComponent<Image>();
        SummonMenuImage = GameObject.Find("UISummon").GetComponent<Image>();
        ItemMenuImage = GameObject.Find("UIItem").GetComponent<Image>();
        AttackMenuImage = GameObject.Find("UIAttack").GetComponent<Image>();
        DefendMenuImage = GameObject.Find("UIDefend").GetComponent<Image>();

        sprite = GameObject.Find("CharacterSprite");
        sprite.SetActive(false);
    }

    private void HideAllImages()
    {
        //reset the scale of all elements and hide them
        //the update loop can then just set the relevant ones without worring about existing elements
        FightMenuImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        FleeMenuImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        DjinnMenuImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        PsyMenuImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        SummonMenuImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        ItemMenuImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        AttackMenuImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        DefendMenuImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        FightMenuImage.enabled = false;
        FleeMenuImage.enabled = false;
        DjinnMenuImage.enabled = false;
        PsyMenuImage.enabled = false;
        SummonMenuImage.enabled = false;
        ItemMenuImage.enabled = false;
        AttackMenuImage.enabled = false;
        DefendMenuImage.enabled = false;
    }


    public void RotateLeft(){
        //testing rotation (TODO: remove)
        rotation += 2;
        if(rotation>360){
            rotation -= 360;
        }
    }
    public void RotateRight()
    {
        //testing rotation (TODO: remove)
        rotation -= 2;
        if (rotation < 0)
        {
            rotation += 360;
        }
    }

    //Given a character, instantiates the rendering sprite 
    //also sets the animation to be the one defined on the character's data
    private GameObject GetCharacterAnimationSprite(Character chara, Transform parent)
    {
        var chSprite = Instantiate(sprite, parent);
        var animator = chSprite.GetComponent<Animator>();
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        foreach (var a in aoc.animationClips)
        {
            //TODO: how to handle missing animations?
            if (a.name == "BattleAttack" && chara.AnimAttack!=null)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, chara.AnimAttack));
            }
            if (a.name == "BattleAttackReverse" && chara.AnimAttackReverse != null)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, chara.AnimAttackReverse));
            }
            if (a.name == "BattleHit" && chara.AnimHit != null)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, chara.AnimHit));
            }
            if (a.name == "BattleHitReverse" && chara.AnimHitReverse != null)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, chara.AnimHitReverse));
            }
            if (a.name == "BattleIdle" && chara.AnimIdle != null)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, chara.AnimIdle));
            }
            if (a.name == "BattleIdleReverse" && chara.AnimIdleReverse != null)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, chara.AnimIdleReverse));
            }
            if (a.name == "BattleSummon" && chara.AnimSummon != null)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, chara.AnimSummon));
            }
            if (a.name == "BattleSummonReverse" && chara.AnimSummonReverse != null)
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, chara.AnimSummonReverse));
            }
        }
        aoc.ApplyOverrides(anims);
        animator.runtimeAnimatorController = aoc;
        return chSprite;
    }

    private void SetCharacterCoordinates (BattleController controller)
    {
        //testing rotation
        //Description of formula:
        //to get "fake" 3d we give each character an X and a Y coordinate
        //allies sit along the X axis, enemies along the Y axis
        //the scene is allowed to rotate around the center of the battle
        //In a top-down game, this would give two parallel lines rotating around a circle
        //to get the "fake" 3d effect, we squish the y-axis, so that instead of orbiting in a circle, it orbits in an ellipse
        //then, to give more depth, scale sprites based on y-axis, so that further sprites are smaller and closer ones bigger

        var characters = controller.GetCharacters();
        var enemies = controller.GetEnemies();
        var allcharacters = new List<Character>();
        //Assign characters their position in the X/Y grid
        float offset = 0;
        var parent = GameObject.Find("BattleCanvas").transform;
        foreach (var chara in characters)
        {
            if (!chImages.ContainsKey(chara.Name))
            {
                var chSprite = GetCharacterAnimationSprite(chara, parent);
                chImages.Add(chara.Name, chSprite);
                chSprite.SetActive(true);
            }
            chara.x = offset;
            chara.y = -orbitHeight;//abount below the center
            offset += characterXOffset; //amount between characters
            allcharacters.Add(chara);
        }
        offset = 0;
        foreach (var e in enemies)
        {
            if (!chImages.ContainsKey(e.Name))
            {
                var chSprite = GetCharacterAnimationSprite(e, parent);
                chImages.Add(e.Name, chSprite);
                chSprite.SetActive(true);
            }
            e.x = offset;
            e.y = orbitHeight;//amount above the center
            offset += characterXOffset;//amount between enemies
            allcharacters.Add(e);
        }

        //quick and dirty way of setting the sprite, a real implementation would allocate sprites dynamically.
        foreach(var ch in allcharacters)
        {
            //convert X/Y to be in an orbit, use trig to calcuate the X/Y's for rotation around a point
            double DEG_TO_RAD = 0.0174533;//trig functions work in radians. Easy way of conversion. PI/180.
            double RAD_TO_DEG = 57.2958;//other way around
            double angle = rotation * DEG_TO_RAD;

            int actionState = (int)ANIMATION_STATE.A_IDLE;
            int actionStateReverse = (int)ANIMATION_STATE.A_IDLE_REVERSE;
            var xCoord = ch.x;
            var yCoord = ch.y;
            //Apply lerp movement if attacking
            //TODO: modularise this 
            if (controller.CurrentState == controller.sRenderBattle)
            {
                //TODO: summon, hit, etc
                var renderTarget = controller.sRenderBattle.RenderTargets.FirstOrDefault();
                if (ch == renderTarget)
                {
                    var skill = renderTarget.ChosenSkill;
                    //if the skill moves the character
                    if(skill.Animation == ANIMATION_STATE.A_ATTACK||
                       skill.Animation == ANIMATION_STATE.A_ATTACK_REVERSE)
                    {
                        var targetName = renderTarget.ChosenTarget;
                        //can't show attack animations for allies 
                        if (skill.Target == TargetType.ENEMY)
                        {
                            var enemy = controller.GetEnemies().Where(x => x.Name == targetName).FirstOrDefault();
                            var frame = controller.sRenderBattle._renderFrames[0];
                            if (enemy != null )//TODO: frame rate is hard-coded. need to change.
                            {
                                if (frame < 50.0f)
                                {
                                    var lerpOrigin = new Vector3(ch.x, ch.y, 0);
                                    var lerpDestination = new Vector3(enemy.x, enemy.y, 0);
                                    var lerp = Vector3.Lerp(lerpDestination, lerpOrigin, ((float)frame) / 50.0f);
                                    xCoord = lerp.x;
                                    yCoord = lerp.y;
                                    actionState = (int)ANIMATION_STATE.A_ATTACK;
                                    actionStateReverse = (int)ANIMATION_STATE.A_ATTACK_REVERSE;
                                }
                            }
                        }
                    }
                }
            }

            double render_x = Math.Cos(angle) * (xCoord - center_x) - Math.Sin(angle) * (yCoord - center_y) + center_x;
            double render_y = Math.Sin(angle) * (xCoord - center_x) + Math.Cos(angle) * (yCoord - center_y) + center_y;

            //now need to work out the angle from their renderd point to the middle to set the sprite
            double angleToMiddleRads = Math.Atan2(render_y - center_y, render_x - center_x);
            var angleToMiddle = angleToMiddleRads * RAD_TO_DEG;
            angleToMiddle = (angleToMiddle+360) % 360;
            angleToMiddle += 90;
            angleToMiddle = angleToMiddle % 360;
            var chImg = chImages[ch.Name];
            var anim = chImg.GetComponent<Animator>();
            anim.SetInteger("State", actionState);
            float horizontalScale = 1.0f;
            if (angleToMiddle > 0 && angleToMiddle <= 90)
            {
                anim.SetInteger("State", actionState);
            }
            if (angleToMiddle > 90 && angleToMiddle <= 180)
            {
                anim.SetInteger("State", actionStateReverse);
            }
            if (angleToMiddle > 180 && angleToMiddle <= 270)
            {
                anim.SetInteger("State", actionStateReverse);
                horizontalScale = -1.0f;
            }
            if (angleToMiddle > 270 && angleToMiddle <= 360)
            {
                anim.SetInteger("State", actionState);
                horizontalScale = -1.0f;
            }
            //above, the y values range between +/-orbitHeight so work out as a percentage how far up the image is
            double yPercent = (render_y + orbitHeight) / (orbitHeight*2);
            render_y = render_y * 0.2;//squish the y axis to fake depth. this represents an ellipse with width 1 and height 0.2
            //this is for scaling in the z-axis.
            //reduce the percentage range so images don't get too small
            yPercent = yPercent / 3;
            //finally, compute the resulting value as being from 0.75 (far away) to 1 (close)
            yPercent = 1 - yPercent;
            //depth = y to make more distant objects render behind closer ones
            chImg.transform.position = new Vector3((float)render_x , (float)render_y , (float)render_y);
            chImg.transform.localScale = new Vector3(horizontalScale* (float)yPercent * spriteScaleFactor, (float)yPercent* spriteScaleFactor, 1);
        }
    }

    public void Render(BattleController controller)
    {
        HideAllImages();//by default hide everything and reset scales
        SetCharacterCoordinates(controller);
        //the main render loop will enable and scale just the releveant elements
        TextElement.text = "";
        if (controller.CurrentState == controller.sDjinnMenu)
        {
            string debugLog = "State: Djinn Menu\n";
            int i = 0;
            var djinn = controller.GetCurrentCharacter().GetDjinn();
            foreach (var item in controller.sDjinnMenu.menuOption)
            {
                if (i > 0)
                {
                    debugLog += "\n";
                }
                if (i == controller.sDjinnMenu.selectedMenuOption)
                {
                    debugLog += "-";
                }
                var optDjinn = djinn.Where(x => x.Name == item).FirstOrDefault();
                if (optDjinn != null)
                {
                    debugLog += optDjinn.Element;
                    debugLog += "(" + optDjinn.State + " " + Convert.ToString(optDjinn.Cooldown) + ")";
                }
                debugLog += item;
                i += 1;
            }
            TextElement.text += debugLog;
            //Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sFightMenu)
        {
            string debugLog = "State: Fight Menu\n";
            debugLog += controller.GetCurrentCharacter().Name + ":";
            int i = 0;
            float itemPos = 0;
            foreach (var item in controller.sFightMenu.menuOption)
            {
                if (i > 0)
                {
                    debugLog += ",";
                }
                bool isSelected = false;
                if (i == controller.sFightMenu.selectedMenuOption)
                {
                    isSelected = true;
                    debugLog += "-";
                }
                if (item == StateFightMenu.MENU_DEFEND)
                {
                    if (isSelected)
                    {
                        DefendMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    }
                    DefendMenuImage.enabled = true;
                    DefendMenuImage.transform.localPosition = new Vector3(itemPos, DefendMenuImage.transform.localPosition.y, 0);
                }
                if (item == StateFightMenu.MENU_SUMMON)
                {
                    if (isSelected)
                    {
                        SummonMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    }
                    SummonMenuImage.enabled = true;
                    SummonMenuImage.transform.localPosition = new Vector3(itemPos, SummonMenuImage.transform.localPosition.y, 0);
                }
                if (item == StateFightMenu.MENU_FIGHT)
                {
                    if (isSelected)
                    {
                        AttackMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    }
                    AttackMenuImage.enabled = true;
                    AttackMenuImage.transform.localPosition = new Vector3(itemPos, AttackMenuImage.transform.localPosition.y, 0);
                }
                if (item == StateFightMenu.MENU_DJINN)
                {
                    if (isSelected)
                    {
                        DjinnMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    }
                    DjinnMenuImage.enabled = true;
                    DjinnMenuImage.transform.localPosition = new Vector3(itemPos, DjinnMenuImage.transform.localPosition.y, 0);
                }
                if (item == StateFightMenu.MENU_ITEM)
                {
                    if (isSelected)
                    {
                        ItemMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    }
                    ItemMenuImage.enabled = true;
                    ItemMenuImage.transform.localPosition = new Vector3(itemPos, ItemMenuImage.transform.localPosition.y, 0);
                }
                if (item == StateFightMenu.MENU_PSY)
                {
                    if (isSelected)
                    {
                        PsyMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    }
                    PsyMenuImage.enabled = true;
                    PsyMenuImage.transform.localPosition = new Vector3(itemPos, PsyMenuImage.transform.localPosition.y, 0);
                }
                debugLog += item;
                i += 1;
                itemPos += 20;
            }
            TextElement.text += debugLog;
            //Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sItemMenu)
        {
            string debugLog = "State: Item Menu\n";
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
            TextElement.text += debugLog;
            //Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sSummonMenu)
        {
            string debugLog = "State: Summon Menu\n";
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
            TextElement.text += debugLog;
            //Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sMainMenu)
        {
            FightMenuImage.enabled = true;
            FleeMenuImage.enabled = true;
            if (controller.sMainMenu.selectedMenuOption == StateMainMenu.MENU_OPTION_FIGHT)
            {
                FightMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
            if (controller.sMainMenu.selectedMenuOption == StateMainMenu.MENU_OPTION_FLEE)
            {
                FleeMenuImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
            string debugLog = "State: Main Menu\n";
            if (controller.sMainMenu.selectedMenuOption == StateMainMenu.MENU_OPTION_FIGHT)
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
            TextElement.text += debugLog;
            //Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sPsyMenu)
        {
            string debugLog = "State: Psy Menu\n";
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
            TextElement.text += debugLog;
            //Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sSelectTarget)
        {
            string debugLog = "State: Select Target\n";
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
            TextElement.text += debugLog;
            //Debug.Log(debugLog);
        }
        if (controller.CurrentState == controller.sFinaliseCharacter)
        {
            //shouldn't get here (or at most for 1 frame)
            //in multiplayer (not yet impleneted) this can persist for some time, 
            //so a message needs to be shown that we are waiting on the opponent
            TextElement.text += ("State: Select Finalise Character");
            TextElement.text += ("State: Waiting on enemy input...");
        }
        if (controller.CurrentState == controller.sRenderBattle)
        {
            //Debug.Log("State:  Render Battle");
            string debugLog = "";
            var characters = controller.sRenderBattle.RenderTargets;
            if (characters.Count > 0)
            {
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
            TextElement.text += debugLog;
            //Debug.Log(debugLog);
            controller.sRenderBattle.ProgressFrame(1);
        }

    }
}
