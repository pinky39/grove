namespace Grove.Core.Effects
{
  using Grove.Core.Targeting;

  public class TargetPlayerGainsLife : Effect
  {
    public int Amount { get; set; }

    protected override void ResolveEffect()
    {
      Target().Player().Life += Amount;
    }
  }
}