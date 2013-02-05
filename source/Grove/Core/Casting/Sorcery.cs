namespace Grove.Core.Casting
{
  using System;
  using Effects;

  public class Sorcery : CastingRule
  {
    private readonly Action<Card> _afterResolvePutToZone;

    private Sorcery() {}

    public Sorcery(Action<Card> afterResolvePutToZone = null)
    {
      _afterResolvePutToZone = afterResolvePutToZone ?? (card => card.PutToGraveyard());
    }

    public override bool CanCast()
    {
      return Game.Turn.Step.IsMain() &&
        Card.Controller.IsActive &&
          Game.Stack.IsEmpty;
    }

    public override void Cast(Effect effect)
    {
      Game.Stack.Push(effect);
    }

    public override void AfterResolve()
    {
      _afterResolvePutToZone(Card);
    }
  }
}