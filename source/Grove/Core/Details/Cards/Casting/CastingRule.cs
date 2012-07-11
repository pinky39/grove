namespace Grove.Core.Details.Cards.Casting
{
  using Effects;
  using Infrastructure;

  [Copyable]
  public abstract class CastingRule
  {
    //protected CastingRule(Card card, Stack stack)
    //{
    //  Stack = stack;
    //  Card = card;
    //}

    //protected CastingRule() {}

    //protected Card Card { get; private set; }
    //protected Player Controller { get { return Card.Controller; } }
    //protected Stack Stack { get; private set; }

    public abstract bool CanCast(Card card);
    public abstract void Cast(Effect effect);
    //{
    //  Stack.Push(effect);
    //}
  }
}