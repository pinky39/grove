namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;
  using Core.Zones;

  public class DarkHatchling : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Dark Hatchling")
        .ManaCost("{4}{B}{B}")
        .Type("Creature Horror")
        .Text(
          "{Flying}{EOL}When Dark Hatchling enters the battlefield, destroy target nonblack creature. It can't be regenerated.")
        .Power(3)
        .Toughness(3)
        .Timing(Timings.OpponentHasPermanent(card =>
          card.Is().Creature &&
            !card.HasColors(ManaColors.Black) &&
              !card.HasProtectionFrom(ManaColors.Black)))
        .Abilities(
          TriggeredAbility(
            "When Dark Hatchling enters the battlefield, destroy target nonblack creature. It can't be regenerated.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<DestroyTargetPermanents>(e => e.AllowRegenerate = false),
            TargetValidator(
              TargetIs.Card(card => card.Is().Creature && !card.HasColors(ManaColors.Black)),
              ZoneIs.Battlefield()),
            selectorAi: TargetSelectorAi.Destroy(),
            abilityCategory: EffectCategories.Destruction));
    }
  }
}