namespace Grove.Core.Cards.Effects
{
  public class EachPlayerDiscardsHand : Effect
  {
    protected override void ResolveEffect()
    {
      Players.Active.DiscardHand();
      Players.Passive.DiscardHand();
    }
  }
}