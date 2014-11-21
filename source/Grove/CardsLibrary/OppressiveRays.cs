namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class OppressiveRays : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Oppressive Rays")
        .ManaCost("{W}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}Enchanted creature can't attack or block unless its controller pays {3}.{EOL}Activated abilities of enchanted creature cost {3} more to activate.")
        .Cast(p =>
        {
          p.Effect = () => new Attach(
            () => new AddStaticAbility(Static.CannotAttack),
            () => new AddStaticAbility(Static.CannotBlock),
            () => new AddCostModifier(new AbilityCostModifier(3.Colorless(), less: false)),
            () =>
            {
              var tp = new TriggeredAbility.Parameters
              {
                Text = "Enchanted creature can't attack unless its controller pays {3}.",
                Effect = () => new PayManaApplyToCard(3.Colorless(),
                  modifier: () => new RemoveAbility(Static.CannotAttack) {UntilEot = true},
                  message: "Pay mana to attack?")
              };

              // Enchanted card's controller can only attack
              tp.Trigger(new OnStepStart(Step.BeginningOfCombat));

              return new AddTriggeredAbility(new TriggeredAbility(tp));
            },
            () =>
            {
              var tp = new TriggeredAbility.Parameters
              {
                Text = "Enchanted creature can't block unless its controller pays {3}.",
                Effect = () => new PayManaApplyToCard(3.Colorless(),
                  modifier: () => new RemoveAbility(Static.CannotBlock) {UntilEot = true},
                  message: "Pay mana to block?")
              };

              // Enchanted card's controller can only block
              tp.Trigger(new OnStepStart(Step.BeginningOfCombat, activeTurn: false, passiveTurn: true));

              return new AddTriggeredAbility(new TriggeredAbility(tp));
            }).SetTags(EffectTag.CombatDisabler);          

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectCannotBlockAttack());
        });
    }
  }
}
