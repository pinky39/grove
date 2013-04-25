namespace Grove.Gameplay.Effects
{
  using Grove.Infrastructure;
  using Targeting;

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

      if (_returnOwningCard && ValidEffectTargets.None(x => x == Source.OwningCard))
      {
        Source.OwningCard.PutToHand();
      }

      if (_discard > 0)
      {
        Enqueue<Decisions.DiscardCards>(
          controller: Target.Card().Controller,
          init: p => p.Count = _discard);
      }
    }
  }
}