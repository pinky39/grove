namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class AcademyResearchers : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Academy Researchers")
        .ManaCost("{1}{U}{U}")
        .Type("Creature Human Wizard")
        .Text(
          "When Academy Researchers enters the battlefield, you may put an Aura card from your hand onto the battlefield attached to Academy Researchers.")
        .Power(2)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          TriggeredAbility(
            "When Academy Researchers enters the battlefield, you may put an Aura card from your hand onto the battlefield attached to Academy Researchers.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<EnchantOwnerWithTarget>(),
            effectValidator: TargetValidator(
              TargetIs.Card(p => p.Card.Is().Aura && p.Card.CanTarget(p.Source)),
              ZoneIs.OwnersHand(),
              minCount: 0),
            selectorAi: TargetSelectorAi.AttachToSource())
        );
    }
  }
}