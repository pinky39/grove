namespace Grove.Gameplay.Decisions.Scenario
{
  using System;
  using Results;

  [Serializable]
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