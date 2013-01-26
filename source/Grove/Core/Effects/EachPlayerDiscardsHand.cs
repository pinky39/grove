namespace Grove.Core.Effects
{
  public class EachPlayerDiscardsHand : Effect
  {
    protected override void ResolveEffect()
    {
      Core.Players.Active.DiscardHand();
      Core.Players.Passive.DiscardHand();
    }
  }
}