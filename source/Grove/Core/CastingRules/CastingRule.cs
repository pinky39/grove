namespace Grove.Core.CastingRules
{
  using Effects;
  using Infrastructure;
  using Zones;

  [Copyable]
  public abstract class CastingRule
  {
    protected CastingRule(Card card, Stack stack)
    {
      Stack = stack;
      Card = card;
    }

    protected CastingRule() {}

    protected Card Card { get; private set; }

    protected Player Controller { get { return Card.Controller; } }

    protected Stack Stack { get; private set; }

    public abstract bool CanCast();

    public virtual void Cast(Effect effect)
    {
      Stack.Push(effect);
    }
  }
}