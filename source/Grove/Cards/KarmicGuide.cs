namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class KarmicGuide : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Karmic Guide")
        .ManaCost("{3}{W}{W}")
        .Type("Creature Angel Spirit")
        .Text(
          "{Flying}, {protection from black}{EOL}When Karmic Guide enters the battlefield, return target creature card from your graveyard to the battlefield.{EOL}{Echo} {3}{W}{W}")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .Protections(CardColor.Black)
        .Echo("{3}{W}{W}")        
        .Cast(p => p.TimingRule(new ControllerGraveyardCountIs(c => c.Is().Creature, minCount: 1)))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Karmic Guide enters the battlefield, return target creature card from your graveyard to the battlefield.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new PutTargetsToBattlefield();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().In.YourGraveyard());
            p.TargetingRule(new OrderByRank(c => -c.Score));
          });
    }
  }
}