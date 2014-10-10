namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class FeastOnTheFallen : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Feast on the Fallen")
        .ManaCost("{2}{B}")
        .Type("Enchantment")
        .Text("At the beginning of each upkeep, if an opponent lost life last turn, put a +1/+1 counter on target creature you control.")
        .FlavorText("\"As our numbers dwindle, the ranks of the dead grow ever stronger.\"{EOL}—Thalia, Knight-Cathar")
        .TriggeredAbility(p =>
        {
          p.Text = "At the beginning of each upkeep, if an opponent lost life last turn, put a +1/+1 counter on target creature you control.";

          p.Trigger(new OnStepStart(activeTurn: true, passiveTurn: true, step: Step.Upkeep)
          {
            Condition = (t, g) => g.Turn.PrevTurnEvents.HasLostLifeFor(t.Controller.Opponent)
          });

          p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(
              () => new PowerToughness(1, 1), count: 1)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature(controlledBy: ControlledBy.SpellOwner).On.Battlefield());
          p.TargetingRule(new EffectCombatEnchantment());

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
