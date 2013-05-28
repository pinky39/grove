namespace Grove.Artifical.Decisions
{
  using System;

  [Serializable]
  public class ChooseEffectOptions : Gameplay.Decisions.ChooseEffectOptions
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(Choices);
    }
  }
}