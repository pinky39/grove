namespace Grove.Core.Decisions.Scenario
{
  using Gameplay.Decisions.Results;

  public class SetTriggeredAbilityTarget : Gameplay.Decisions.SetTriggeredAbilityTarget, IScenarioDecision
  {
    public static SetTriggeredAbilityTarget None
    {
      get
      {
        return new SetTriggeredAbilityTarget
          {
            Result = new ChosenTargets(null)
          };
      }
    }

    public bool CanExecute()
    {
      return true;
    }

    protected override void ExecuteQuery() {}
  }
}