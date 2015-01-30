namespace Grove.Effects
{
  public class EachPlayerDiscardsHandAndDrawsThatManyCardsMinusOne : Effect
  {
    protected override void ResolveEffect()
    {
      var count = Players.Active.Hand.Count;

      Players.Active.DiscardHand();
      Players.Active.DrawCards(count - 1);

      count = Players.Passive.Hand.Count;

      Players.Passive.DiscardHand();
      Players.Passive.DrawCards(count - 1);
    }
  }
}
