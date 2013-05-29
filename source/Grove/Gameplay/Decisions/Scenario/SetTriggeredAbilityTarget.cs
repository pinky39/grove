namespace Grove.Gameplay.Decisions.Scenario
{
  using Results;

  public class SetTriggeredAbilityTarget : Decisions.SetTriggeredAbilityTarget, IScenarioDecision
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