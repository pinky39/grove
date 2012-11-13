namespace Grove.Core.Cards.Casting
{
  using Effects;
  using Grove.Core.Zones;

  public class Instant : CastingRule
  {
    private readonly Stack _stack;

    public Instant(Stack stack)
    {
      _stack = stack;
    }

    private Instant() {}

    public override bool CanCast(Card card)
    {
      return card.CanPayCastingCost();
    }

    public override void Cast(Effect effect)
    {
      _stack.Push(effect);
    }
  }
}