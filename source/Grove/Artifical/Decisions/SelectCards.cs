namespace Grove.Artifical.Decisions
{
  using System;

  [Serializable]
  public class SelectCards : Gameplay.Decisions.SelectCards
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(ValidTargets);
    }
  }
}