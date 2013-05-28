namespace Grove.Artifical.Decisions
{
  using System;

  [Serializable]
  public class ChooseTo : Gameplay.Decisions.ChooseTo
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult();
    }
  }
}