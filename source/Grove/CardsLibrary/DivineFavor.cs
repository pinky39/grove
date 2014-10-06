namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class DivineFavor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Divine Favor")
        .ManaCost("{1}{W}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}When Divine Favor enters the battlefield, you gain 3 life.{EOL}Enchanted creature gets +1/+3.")
        .FlavorText("With an armory of light, even the squire may champion her people.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(1, 3))
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreasePower);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Divine Favor enters the battlefield, you gain 3 life.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ChangeLife(amount: 3, forYou: true);
          });
    }
  }
}