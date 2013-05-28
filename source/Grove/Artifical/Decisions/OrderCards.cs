namespace Grove.Artifical.Decisions
{
  using System;

  [Serializable]
  public class OrderCards : Gameplay.Decisions.OrderCards
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(Cards);
    }
  }
}