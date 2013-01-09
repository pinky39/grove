namespace Grove.Core.Cards.Effects
{
  using Targeting;

  public class TargetPlayerGainsLife : Effect
  {
    public int Amount { get; set; }

    protected override void ResolveEffect()
    {
      Target().Player().Life += Amount;
    }
  }
}