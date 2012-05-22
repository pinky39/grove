namespace Grove.Core.CastingRules
{
  using Zones;

  public class Sorcery : CastingRule
  {
    private readonly TurnInfo _turn;

    public Sorcery(Card card, Stack stack, TurnInfo turn)
      : base(card, stack)
    {
      _turn = turn;
    }

    private Sorcery() {}

    public override bool CanCast()
    {
      return
        _turn.Step.IsMain() &&
          Controller.IsActive &&
            Stack.IsEmpty &&
              Controller.HasEnoughMana(Card.ManaCost);
    }
  }
}