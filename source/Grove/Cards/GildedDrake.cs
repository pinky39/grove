namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class GildedDrake : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Gilded Drake")
        .ManaCost("{1}{U}")
        .Type("Creature Drake")
        .Text(
          "{Flying}{EOL}When Gilded Drake enters the battlefield, exchange control of Gilded Drake and up to one target creature an opponent controls. If you don't make an exchange, sacrifice Gilded Drake. This ability can't be countered except by spells and abilities.")
        .FlavorText("Buyer beware.")
        .Power(3)
        .Toughness(3)
        .Timing(Timings.FirstMain())
        .Abilities(
          Static.Flying,
          TriggeredAbility(
            "When Gilded Drake enters the battlefield, exchange control of Gilded Drake and up to one target creature an opponent controls. If you don't make an exchange, sacrifice Gilded Drake. This ability can't be countered except by spells and abilities.",
            Trigger<OnZoneChange>(t => { t.To = Zone.Battlefield; }),
            Effect<ExchangeForOpponentsCreature>(),
            abilityCategory: EffectCategories.Destruction,
            selectorAi: TargetSelectorAi.GainControl(),
            effectValidator: TargetValidator(
              TargetIs.Card(x => x.Is().Creature, Controller.Opponent),
              ZoneIs.Battlefield()),            
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}