namespace Grove.Core.Cards.Casting
{
  using Effects;
  using Grove.Core.Zones;

  public class Default : CastingRule
  {
    private readonly Stack _stack;
    private readonly TurnInfo _turn;

    public Default(Stack stack, TurnInfo turn)
    {
      _stack = stack;
      _turn = turn;
    }

    private Default() {}

    public override bool CanCast(Card card)
    {
      return _turn.Step.IsMain() &&
        card.Controller.IsActive &&
          _stack.IsEmpty &&
            card.CanPayCastingCost();
    }

    public override void Cast(Effect effect)
    {
      _stack.Push(effect);
    }
  }
}