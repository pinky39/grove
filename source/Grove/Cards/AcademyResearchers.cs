namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class AcademyResearchers : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Academy Researchers")
        .ManaCost("{1}{U}{U}")
        .Type("Creature Human Wizard")
        .Text(
          "When Academy Researchers enters the battlefield, you may put an Aura card from your hand onto the battlefield attached to Academy Researchers.")
        .Power(2)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          C.TriggeredAbility(
           "When Academy Researchers enters the battlefield, you may put an Aura card from your hand onto the battlefield attached to Academy Researchers.",
           C.Trigger<OnZoneChange>((t, _) => t.To = Zone.Battlefield),
           C.Effect<EnchantOwnerWithTarget>(),
           effectValidator: C.Validator(
            Validators.CardInHand(p => p.Target.Is().Aura && p.Target.Card().CanTarget(p.Source)), minCount: 0),
           selectorAi: TargetSelectorAi.AttachToSource()
          )
        );
    }
  }
}