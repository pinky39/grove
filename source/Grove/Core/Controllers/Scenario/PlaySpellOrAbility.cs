namespace Grove.Core.Controllers.Scenario
{
  using Results;

  public class PlaySpellOrAbility : Controllers.PlaySpellOrAbility, IScenarioDecision
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