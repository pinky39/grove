namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class IntrepidHero : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Intrepid Hero")
        .ManaCost("{2}{W}")
        .Type("Creature Human Soldier")
        .Text("{T} : Destroy target creature with power 4 or greater.")
        .FlavorText(
          "'We each have our own strengths, Radiant,' Serra said with a sly smile. 'If all of my people were like this one, who would carry your scrolls?'")
        .Power(1)
        .Toughness(1)        
        .Abilities(
          ActivatedAbility(
            "{T} : Destroy target creature with power 4 or greater.",
            Cost<Tap>(),
            Effect<DestroyTargetPermanents>(),            
            Target(
              Validators.Card(card => card.Is().Creature && card.Power >= 4),
              Zones.Battlefield()),
            targetingAi: TargetingAi.Destroy(),
            timing: Timings.InstantRemovalTarget()));
    }
  }
}