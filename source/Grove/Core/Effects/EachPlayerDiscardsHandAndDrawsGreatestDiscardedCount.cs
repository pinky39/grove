namespace Grove.Effects
{
  using System;

  public class EachPlayerDiscardsHandAndDrawsGreatestDiscardedCount : Effect
  {
    protected override void ResolveEffect()
    {
      var maxCount = Math.Max(Players.Player1.Hand.Count, Players.Player2.Hand.Count);

      Players.Active.DiscardHand();
      Players.Active.DrawCards(maxCount);

      Players.Passive.DiscardHand();
      Players.Passive.DrawCards(maxCount);
    }
  }
}