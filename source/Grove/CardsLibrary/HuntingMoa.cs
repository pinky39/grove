namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class HuntingMoa : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hunting Moa")
        .ManaCost("{2}{G}")
        .Type("Creature Bird Beast")
        .Text(
          "{Echo} {2}{G}{EOL}When Hunting Moa enters the battlefield or dies, put a +1/+1 counter on target creature.")
        .Power(3)
        .Toughness(2)
        .Echo("{2}{G}")
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Hunting Moa enters the battlefield or dies, put a +1/+1 counter on target creature.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard));

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield));

            p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(
              () => new PowerToughness(1, 1), count: 1));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}