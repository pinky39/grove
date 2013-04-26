namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Ai.TargetingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Player;
  using Gameplay.Zones;

  public class GildedDrake : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Gilded Drake enters the battlefield, exchange control of Gilded Drake and up to one target creature an opponent controls. If you don't make an exchange, sacrifice Gilded Drake. This ability can't be countered except by spells and abilities.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ExchangeForOpponentsCreature {Category = EffectCategories.Destruction};
            p.TargetSelector.AddEffect(trg => trg.Is.Creature(ControlledBy.Opponent).On.Battlefield());
            p.TargetingRule(new GainControl());
          });
    }
  }
}