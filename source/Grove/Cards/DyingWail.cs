namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class DyingWail : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dying Wail")
        .ManaCost("{1}{B}")
        .Type("Enchantment Aura")
        .Text("When enchanted creature dies, target player discards two cards.")
        .FlavorText("This is a world of spiteful wasps that sting and kill even as they die.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectRankBy(c => c.Score, ControlledBy.SpellOwner));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When enchanted creature dies, target player discards two cards.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, g) => a.SourceCard.AttachedTo == c));

            p.Effect = () => new DiscardCards(2);
            p.TargetSelector.AddEffect(s => s.Is.Player());
            p.TargetingRule(new EffectOpponent());

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}