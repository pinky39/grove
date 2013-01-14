namespace Grove.Core.Casting
{
  using System;
  using Effects;

  public class Instant : CastingRule
  {
    public Action<Card> AfterResolvePutToZone = card => card.PutToGraveyard();    

    public override bool CanCast()
    {
      return true;
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