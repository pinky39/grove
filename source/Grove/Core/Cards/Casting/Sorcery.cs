namespace Grove.Core.Cards.Casting
{
  using System;
  using Effects;

  public class Sorcery : CastingRule
  {
    public Action<Card> AfterResolvePutToZone = card => card.PutToGraveyard();

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
      AfterResolvePutToZone(Card);
    }
  }
}