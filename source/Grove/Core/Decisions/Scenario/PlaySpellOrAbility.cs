namespace Grove.Core.Decisions.Scenario
{
  using Results;

  public class PlaySpellOrAbility : Decisions.PlaySpellOrAbility, IScenarioDecision
  {
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
      return Result.Playable.CanPlay();
    }

    protected override void ExecuteQuery() {}
  }
}