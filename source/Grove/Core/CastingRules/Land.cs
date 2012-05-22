namespace Grove.Core.CastingRules
{
  using Effects;
  using Zones;

  public class Land : CastingRule
  {
    private readonly TurnInfo _turn;

    public Land(Card card, Stack stack, TurnInfo turn) : base(card, stack)
    {
      _turn = turn;
    }

    private Land() {}

    public override bool CanCast()
    {
      return
        _turn.Step.IsMain() &&
          Controller.IsActive &&
            Stack.IsEmpty &&
              Controller.CanPlayLands;
    }    

    public override void Cast(Effect effect)
    {
      effect.Resolve();
    }
  }
}