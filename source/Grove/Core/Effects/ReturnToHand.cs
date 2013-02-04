namespace Grove.Core.Effects
{
  using Targeting;
  using Zones;

  public class ReturnToHand : Effect
  {
    private readonly int _discard;
    private readonly bool _returnOwningCard;

    private ReturnToHand() {}

    public ReturnToHand(int discard = 0, bool returnOwningCard = false)
    {
      _discard = discard;
      _returnOwningCard = returnOwningCard;
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().PutToHand();
      }

      if (_returnOwningCard && Source.OwningCard.Zone == Zone.Battlefield)
      {
        Source.OwningCard.PutToHand();
      }

      if (_discard > 0)
      {
        Game.Enqueue<Decisions.DiscardCards>(
          controller: Target.Card().Controller,
          init: p => p.Count = _discard);
      }
    }
  }
}