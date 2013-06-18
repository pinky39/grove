namespace Grove.Gameplay.CastingRules
{
  using System;

  public class Instant : CastingRule
  {
    private readonly Action<Card> _afterResolvePutToZone;

    private Instant() {}

    public Instant(Action<Card> afterResolvePutToZone = null)
    {
      _afterResolvePutToZone = afterResolvePutToZone ?? (card => card.PutToGraveyard());
    }

    public override bool CanCast()
    {
      return true;
    }

    public override void AfterResolve()
    {
      _afterResolvePutToZone(Card);
    }
  }
}