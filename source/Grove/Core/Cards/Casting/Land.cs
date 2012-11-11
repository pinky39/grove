namespace Grove.Core.Details.Cards.Casting
{
  using Effects;
  using Zones;

  public class Land : CastingRule
  {
    private readonly Stack _stack;
    private readonly TurnInfo _turn;

    public Land(Stack stack, TurnInfo turn)
    {
      _stack = stack;
      _turn = turn;
    }

    private Land() {}

    public override bool CanCast(Card card)
    {
      return _turn.Step.IsMain() &&
        card.Controller.IsActive &&
          _stack.IsEmpty &&
            card.Controller.CanPlayLands &&
              card.CanPayCastingCost();
    }

    public override void Cast(Effect effect)
    {
      effect.Resolve();
      effect.FinishResolve();
    }
  }
}