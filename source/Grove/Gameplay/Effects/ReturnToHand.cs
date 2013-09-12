namespace Grove.Gameplay.Effects
{
  using Artifical;
  using Infrastructure;
  using Targeting;
  using Zones;

  public class ReturnToHand : Effect
  {
    private readonly int _discard;
    private readonly bool _returnOwningCard;
    private Zone _owningCardZone;

    private ReturnToHand() {}

    public ReturnToHand(int discard = 0, bool returnOwningCard = false, EffectTag tag = EffectTag.Bounce)
    {
      _discard = discard;
      _returnOwningCard = returnOwningCard;
      
      SetTags(tag);
    }

    protected override void Initialize()
    {
      _owningCardZone = Source.OwningCard.Zone;
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().PutToHand();
      }

      if (_returnOwningCard && ValidEffectTargets.None(x => x == Source.OwningCard))
      {
        Source.OwningCard.PutToHandFrom(_owningCardZone);
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