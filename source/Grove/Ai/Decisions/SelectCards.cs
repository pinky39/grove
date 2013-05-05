namespace Grove.Ai.Decisions
{
  using System;

  public class SelectCards : Gameplay.Decisions.SelectCards
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(ValidTargets);
    }
  }
}