namespace Grove.Core.Decisions.Scenario
{
  using System;
  using Results;

  public class PlaySpellOrAbility : Decisions.PlaySpellOrAbility, IScenarioDecision
  {
    public Func<Card, Game, bool> Condition = delegate { return true; };
    
    public static PlaySpellOrAbility Pass
    {
      get
      {
        return new PlaySpellOrAbility
          {
            Result = new Pass()
          };
      }
    }

    public bool CanExecute()
    {
      return Result.Playable.CanPlay() && Condition(Result.Playable.Card, Game);
    }

    protected override void ExecuteQuery() {}
  }
}