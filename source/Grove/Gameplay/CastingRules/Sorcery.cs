namespace Grove.Gameplay.CastingRules
{
  using System;
  using Effects;
  using States;

  [Serializable]
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
      return Turn.Step.IsMain() &&
        Card.Controller.IsActive &&
          Stack.IsEmpty;
    }

    public override void Cast(Effect effect)
    {
      Stack.Push(effect);
    }

    public override void AfterResolve()
    {
      _afterResolvePutToZone(Card);
    }
  }
}