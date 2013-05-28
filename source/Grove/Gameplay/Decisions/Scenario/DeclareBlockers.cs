namespace Grove.Gameplay.Decisions.Scenario
{
  using System;
  using Results;

  [Serializable]
  public class DeclareBlockers : Decisions.DeclareBlockers, IScenarioDecision
  {
    public static DeclareBlockers None
    {
      get
      {
        return new DeclareBlockers
          {
            Result = new ChosenBlockers()
          };
      }
    }

    protected override bool ShouldExecuteQuery { get { return true; } }

    public bool CanExecute()
    {
      return true;
    }

    protected override void ExecuteQuery() {}
  }
}