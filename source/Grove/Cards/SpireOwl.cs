namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class SpireOwl : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Spire Owl")
        .ManaCost("{1}{U}")
        .Type("Creature Bird")
        .Text(
          "{Flying}{EOL}When Spire Owl enters the battlefield, look at the top four cards of your library, then put them back in any order.")
        .Power(1)
        .Toughness(1)
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Spire Owl enters the battlefield, look at the top four cards of your library, then put them back in any order.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ReorderTopCards(4);
          });
    }
  }
}