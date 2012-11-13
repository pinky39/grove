namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Rescind : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Rescind")
        .ManaCost("{1}{U}{U}")
        .Type("Instant")
        .Text("Return target permanent to its owner's hand.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")        
        .Timing(Timings.InstantRemovalTarget())
        .Category(EffectCategories.Bounce)
        .Cycling("{2}")
        .Effect<ReturnToHand>(e =>
          {            
            e.ReturnTarget = true;
          })
        .Targets(
          selectorAi: TargetSelectorAi.Bounce(),
          effectValidator: Validator(Validators.Permanent()));
    }
  }
}