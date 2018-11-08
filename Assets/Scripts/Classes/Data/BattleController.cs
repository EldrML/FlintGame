using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//These are temporary classes for setting up a battle
//TODO: replace it with a real implementation when real data is implemented.


//TOTO: this should probably be renamed "BattleOrchestrator" or something similar.
//Controller implies MVC so may be misleading
public class BattleController : MonoBehaviour {

    [SerializeField]
    private Character[] _characters;
    [SerializeField]
    private Character[] _enemies;

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
        CurrentState = sMainMenu;
        //each character needs a unique name.
        //if instantiating multiples of the same unit, assign them a suffix
        var tempCharacters = new List<Character>();
        var tempEnemies = new List<Character>();
        foreach (var charachter in _characters)
        {
            var ch = Instantiate(charachter);
            if (tempCharacters.Where(x => x.Name == ch.Name).Count() > 0)
            {
                //TODO: replace "startWith" will a proper check, e.g. trim suffix numbers and do equality check
                ch.Name = ch.Name + tempCharacters.Where(x => x.Name.StartsWith(ch.Name)).Count();
            }
            tempCharacters.Add(ch);
        }
        foreach (var enemy in _enemies)
        {
            var e = Instantiate(enemy);
            if (tempEnemies.Where(x => x.Name == e.Name).Count() > 0)
            {
                e.Name = e.Name + tempEnemies.Where(x => x.Name.StartsWith(x.Name)).Count();
            }
            tempEnemies.Add(e);
        }
        _characters = tempCharacters.ToArray();
        _enemies = tempEnemies.ToArray();
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
        return _characters.ToList();
    }
    public List<Character> GetEnemies()
    {
        return _enemies.ToList();
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
            var eCount = summon.Requirements.Count(x => x == Djinn.DjinnElement.ELEMENT_EARTH);
            var fCount = summon.Requirements.Count(x => x == Djinn.DjinnElement.ELEMENT_FIRE);
            var wCount = summon.Requirements.Count(x => x == Djinn.DjinnElement.ELEMENT_WATER);
            var aCount = summon.Requirements.Count(x => x == Djinn.DjinnElement.ELEMENT_AIR);
            var standby = _djinn.Where(x => x.State == Djinn.DjinnState.STATE_STANDBY);
            var djinnEarthCount = standby.Count(x => x.Element == Djinn.DjinnElement.ELEMENT_EARTH);
            var djinnFireCount = standby.Count(x => x.Element == Djinn.DjinnElement.ELEMENT_FIRE);
            var djinnWaterCount = standby.Count(x => x.Element == Djinn.DjinnElement.ELEMENT_WATER);
            var djinnAirCount = standby.Count(x => x.Element == Djinn.DjinnElement.ELEMENT_AIR);
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
            enemy.ChosenSkill = enemy.Weapon;
            if (enemy.Weapon == null)
            {
                enemy.ChosenSkill = Defend.DEFEND;
            }
            enemy.ChosenTarget = _characters.First().Name;
        }
        return true;
    }
}
