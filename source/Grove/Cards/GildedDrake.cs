namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class GildedDrake : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Gilded Drake enters the battlefield, exchange control of Gilded Drake and up to one target creature an opponent controls. If you don't make an exchange, sacrifice Gilded Drake. This ability can't be countered except by spells and abilities.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ExchangeForOpponentsCreature().Tags(EffectTag.ChangeController);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature(ControlledBy.Opponent).On.Battlefield());
            p.TargetingRule(new EffectGainControl());
          });
    }
  }
}