using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//These are temporary classes for setting up a battle
//TODO: replace it with a real implementation when real data is implemented.
public static class RangeType {
	public const string ONE="|";
	public const string THREE=".|.";
	public const string FIVE="..|..";
	public const string ALL="|||||";
}
public static class AttackType{
	public const string HEALING="healing";
	public const string MULTIPLIER="multiplier";
	public const string EFFECT="effect";
	public const string EFFECT_MOVE_FIRST="effectmovefirst";
	public const string DAMAGE="damage";
}
public static class TargetType {
	public const string ALLY="ally";
	public const string ENEMY="enemy";
}
public class Skill
{
    public string Element { get; set; }
    public string Range { get; set; }
    public string AttackType { get; set; }
    public string Target { get; set; }
    public int Power { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //TODO: stats, attacks, etc
}

public class Djinn: Skill
{
    public const string STATE_SET = "set";
    public const string STATE_STANDBY = "standby";
    public const string STATE_SUMMMON = "summon";

    public const string ELEMENT_EARTH = "earth";
    public const string ELEMENT_FIRE = "fire";
    public const string ELEMENT_WATER = "water";
    public const string ELEMENT_AIR = "air";

    public string State { get; set; }
    public int Cooldown { get; set; }
}
public class Psy: Skill
{
    public int Cost { get; set; }
    //TODO: prerequsities
}
public class Summon: Skill{
    public List<string> Requirements { get; set; }
}
public class Item: Skill{}

public class Character {
    public const string SKILL_BASIC_ATTACK = "basicattack";
    public const string SKILL_BASIC_DEFEND = "defend";

    private List<Psy> _psy = new List<Psy>();
    private List<Djinn> _djinn = new List<Djinn>();
    private List<Item> _item = new List<Item>();

    public int pp = 100;
	public int hp = 100;
    public Skill ChosenSkill { get; set; }
    public string chosenTarget = "";

    public Character()
	{
		//TODO: when replacing this with the real implementation, put in some way to load a character
        _psy.Add(new Psy() { Name = "psy 1dmg", Cost = 5, AttackType = AttackType.DAMAGE ,Target=TargetType.ENEMY, Range =  RangeType.ONE});
        _psy.Add(new Psy() { Name = "psy 3dmg", Cost = 15, AttackType = AttackType.DAMAGE , Target=TargetType.ENEMY, Range =  RangeType.THREE });
        _psy.Add(new Psy() { Name = "psy 1debuff", Cost = 15, AttackType = AttackType.EFFECT , Target=TargetType.ENEMY , Range =  RangeType.ONE});
        _psy.Add(new Psy() { Name = "psy 1heal", Cost = 15, AttackType = AttackType.HEALING , Target=TargetType.ALLY , Range =  RangeType.ONE});
        _psy.Add(new Psy() { Name = "psy allBuffFirst", Cost = 15, AttackType = AttackType.EFFECT_MOVE_FIRST , Target=TargetType.ALLY , Range =  RangeType.ALL});
        _psy.Add(new Psy() { Name = "psy 3Buff", Cost = 15, AttackType = AttackType.EFFECT, Target=TargetType.ALLY , Range =  RangeType.THREE});

        _djinn.Add(new Djinn() { Name = "Djinn1 dmg single", State = Djinn.STATE_SET, Element = Djinn.ELEMENT_EARTH, AttackType = AttackType.DAMAGE ,Target=TargetType.ENEMY, Range =  RangeType.ONE });
        _djinn.Add(new Djinn() { Name = "Djinn2 buff first all", State = Djinn.STATE_SET, Element = Djinn.ELEMENT_FIRE, AttackType = AttackType.EFFECT_MOVE_FIRST , Target=TargetType.ALLY , Range =  RangeType.ALL });
        _djinn.Add(new Djinn() { Name = "Djinn3 debuff 1", State = Djinn.STATE_SET, Element = Djinn.ELEMENT_FIRE, AttackType = AttackType.EFFECT , Target=TargetType.ENEMY , Range =  RangeType.ONE });

    }

    public string Name { get; set; }
    public List<Psy> GetPsy()
    {
        return _psy.Where(x=>x.Cost<=pp).ToList();
    }
    public List<Djinn> GetDjinn()
    {
        return _djinn.ToList();
    }
    public List<string> GetItems()
    {
        return new List<string>()
        {
            "Item1",
            "Item2",
            "Item3",
            "Item4"
        };
    }
    public bool CanAttack()
    {
        return true;//false if they don't have a weapon
    }
}

//TOTO: this should probably be renamed "BattleOrchestrator" or something similar.
//Controller implies MVC so may be misleading
public class BattleController : MonoBehaviour {

    private List<Character> _characters = new List<Character>();
    private List<Character> _enemies = new List<Character>();

    public const string ACTION_AGREE = "A";
    public const string ACTION_BACK = "B";
    public const string ACTION_LEFT = "LEFT";
    public const string ACTION_RIGHT = "RIGHT";
    public const string ACTION_UP = "UP";
    public const string ACTION_DOWN = "DOWN";
    public const string ACTION_NONE = "NONE";

    private string currentAction = ACTION_NONE;

    private BattleRenderer battleRenderer = new BattleRenderer();
    
    public StateDjinnMenu sDjinnMenu = new StateDjinnMenu();
    public StateFightMenu sFightMenu = new StateFightMenu();
    public StateFinaliseCharacter sFinaliseCharacter = new StateFinaliseCharacter();
    public StateItemMenu sItemMenu = new StateItemMenu();
    public StateMainMenu sMainMenu = new StateMainMenu();
    public StatePsyMenu sPsyMenu = new StatePsyMenu();
    public StateRenderBattle sRenderBattle = new StateRenderBattle();
    public StateSelectTarget sSelectTarget = new StateSelectTarget();
    public StateSummonMenu sSummonMenu = new StateSummonMenu();

    public BaseState CurrentState { get; set; }

    private int selectedCharacter = 0;

	// Use this for initialization
	void Start () {
        Character ch1 = new Character() { Name = "Ch1" };
        _characters.Add(ch1);
        Character ch2 = new Character() { Name = "Ch2" };
        _characters.Add(ch2);
        Character ch3 = new Character() { Name = "Ch3" };
        _characters.Add(ch3);
        Character e1 = new Character() { Name = "e1" };
        _enemies.Add(e1);
        Character e2 = new Character() { Name = "e2" };
        _enemies.Add(e2);
        Character e3 = new Character() { Name = "e3" };
        _enemies.Add(e3);


        _summon.Add(new Summon() { Name = "Summon earth 1", Range = RangeType.ALL, AttackType = AttackType.DAMAGE, Target = TargetType.ENEMY, Requirements = new List<string>() { Djinn.ELEMENT_EARTH } });
        _summon.Add(new Summon() { Name = "Summon fire 1", Range = RangeType.ALL, AttackType = AttackType.DAMAGE, Target = TargetType.ENEMY, Requirements = new List<string>() { Djinn.ELEMENT_FIRE } });
        _summon.Add(new Summon() { Name = "Summon fire 2", Range = RangeType.ALL, AttackType = AttackType.DAMAGE, Target = TargetType.ENEMY, Requirements = new List<string>() { Djinn.ELEMENT_FIRE, Djinn.ELEMENT_FIRE } });
        _summon.Add(new Summon() { Name = "Summon Fire2Earth1", Range = RangeType.ALL, AttackType = AttackType.DAMAGE, Target = TargetType.ENEMY, Requirements = new List<string>() { Djinn.ELEMENT_FIRE, Djinn.ELEMENT_EARTH } });


        CurrentState = sMainMenu;
	}
	
	// Update is called once per frame
	void Update ()
    {
		//map user input to state actions.
		//there is a 1-1 mapping between actions and keys.
		//states themselves should have no knowledge of physical inputs.
		//note: this assumes keyboard controls only.
		//e.g. If there's mouse controls states will need to handle these as well.
        currentAction = ACTION_NONE;
        if (Input.GetKeyUp(KeyCode.A))
        {
            currentAction = ACTION_AGREE;
        }
        if (Input.GetKeyUp(KeyCode.B)|| Input.GetKeyUp(KeyCode.Z))
        {
            currentAction = ACTION_BACK;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            currentAction = ACTION_LEFT;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            currentAction = ACTION_RIGHT;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            currentAction = ACTION_UP;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            currentAction = ACTION_DOWN;
        }
		//most states don't change without input, so can skip the check most of the time.
        if (currentAction != ACTION_NONE ||
			CurrentState == sFinaliseCharacter ||
			CurrentState == sRenderBattle)
        {
            CurrentState.CheckForStateAndChange(this, currentAction);
            CurrentState.Logic(currentAction);
        }
        battleRenderer.Render(this);//TODO: replace this with a proper renderer and remove from main loop
    }
    public List<Character> GetCharacters()
    {
        return _characters;
    }
    public List<Character> GetEnemies()
    {
        return _enemies;
    }

    public Character GetCurrentCharacter()
    {
        return _characters[selectedCharacter];
    }
    public void SelectPreviousCharacter()
    {
        selectedCharacter -= 1;
        if (selectedCharacter < 0)
        {
            selectedCharacter = 0;
        }
    }
    public void SelectNextCharacter()
    {
        selectedCharacter += 1;
        if (selectedCharacter >= _characters.Count())
        {
            selectedCharacter = _characters.Count() - 1;
        }
    }
    public void SelectFirstCharacter()
    {
        selectedCharacter = 0;
    }

    private List<Summon> _summon = new List<Summon>();
    public List<Summon> GetCharacterSummons()
    {
        //TODO: summons pull from a global djinn pool, not a per-character pool.
        List<Summon> result = new List<Summon>();
        var _djinn = new List<Djinn>();
        foreach(var ch in _characters){
            _djinn.AddRange(ch.GetDjinn());
        }
        foreach (var summon in _summon)
        {
            bool prereqmet = false;
            var eCount = summon.Requirements.Count(x => x == Djinn.ELEMENT_EARTH);
            var fCount = summon.Requirements.Count(x => x == Djinn.ELEMENT_FIRE);
            var wCount = summon.Requirements.Count(x => x == Djinn.ELEMENT_WATER);
            var aCount = summon.Requirements.Count(x => x == Djinn.ELEMENT_AIR);
            var standby = _djinn.Where(x => x.State == Djinn.STATE_STANDBY);
            var djinnEarthCount = standby.Count(x => x.Element == Djinn.ELEMENT_EARTH);
            var djinnFireCount = standby.Count(x => x.Element == Djinn.ELEMENT_FIRE);
            var djinnWaterCount = standby.Count(x => x.Element == Djinn.ELEMENT_WATER);
            var djinnAirCount = standby.Count(x => x.Element == Djinn.ELEMENT_AIR);
            if (djinnEarthCount >= eCount &&
               djinnFireCount >= fCount &&
               djinnWaterCount >= wCount &&
               djinnAirCount >= aCount)
            {
                prereqmet = true;
            }
            if (prereqmet)
            {
                result.Add(summon);
            }
        }

        return result;
    }
    //returns true if the turn is ready to commit enemy actions
    //this should almost always return true, since the enemy actions can be generated by AI on the fly
    //the only time it should return false is during multiplayer, when we need to wait on opponent's choices
    //returning false is not implemented yet.
    public bool EnemyActionsReady () {
        //TODO: AI here (should be pluggable)
        foreach(var enemy in _enemies){
            enemy.ChosenSkill = new Skill() { Name = Character.SKILL_BASIC_ATTACK };
            enemy.chosenTarget = _characters.First().Name;
        }
        return true;
    }
}
