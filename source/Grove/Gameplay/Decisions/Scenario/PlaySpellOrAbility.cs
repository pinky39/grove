namespace Grove.Gameplay.Decisions.Scenario
{
  using System;
  using Results;

  public class PlaySpellOrAbility : Decisions.PlaySpellOrAbility, IScenarioDecision
  {
    public Func<Game, bool> Condition = delegate { return true; };

    public static PlaySpellOrAbility Pass
    {
      get
      {
        return new PlaySpellOrAbility
          {
            Result = new ChosenPlayable {Playable = new Pass()}
          };
      }
    }

    public bool CanExecute()
    {
      return Result.Playable.CanPlay() && Condition(Game);
    }

    protected override void ExecuteQuery() {}
  }
}