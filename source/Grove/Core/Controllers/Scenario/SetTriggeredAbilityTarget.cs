namespace Grove.Core.Controllers.Scenario
{
  using Results;

  public class SetTriggeredAbilityTarget : Controllers.SetTriggeredAbilityTarget, IScenarioDecision
  {
    public static SetTriggeredAbilityTarget None
    {
      get
      {
        return new SetTriggeredAbilityTarget{
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