namespace Grove.Core.Details.Cards.Effects
{
  public class GainLife : Effect
  {
    public int Amount { get; set; }

    protected override void ResolveEffect()
    {
      Controller.Life += Amount;
    }
  }
}