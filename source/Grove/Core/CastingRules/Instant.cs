namespace Grove.Core.CastingRules
{
  using Zones;

  public class Instant : CastingRule
  {
    public Instant(Card card, Stack stack) : base(card, stack) {}
    private Instant() {}

    public override bool CanCast()
    {
      return Controller.HasEnoughMana(Card.ManaCost);
    }
  }
}