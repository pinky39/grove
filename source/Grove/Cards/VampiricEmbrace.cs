namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class VampiricEmbrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vampiric Embrace")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature gets +2/+2 and has flying.{EOL}Whenever a creature dealt damage by enchanted creature this turn dies, put a +1/+1 counter on that creature.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a creature dealt damage by enchanted creature this turn dies, put a +1/+1 counter on that creature.";

            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard,
              filter: (c, a, g) => g.Turn.Events.HasBeenDamagedBy(c, a.OwningCard.AttachedTo)));

            p.Effect = () => new ApplyModifiersToSelf(modifiers: () => new AddCounters(() => new PowerToughness(1, 1), 1), toAttachedTo: true);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddStaticAbility(Static.Flying))
              {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}