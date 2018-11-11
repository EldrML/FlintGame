public abstract class BaseState  {

    //return true if there is a transition, false otherwise
    public abstract void CheckForStateAndChange(BattleController controller, string action);

    //handles game logic for arrow keys which cannot alter the states
    //also handles cases where a/b do not change state
    public abstract void Logic(string action);

}
