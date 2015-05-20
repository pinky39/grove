namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class QuietContemplation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Quiet Contemplation")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text(
          "Whenever you cast a noncreature spell, you may pay {1}. If you do, tap target creature an opponent controls and it doesn't untap during its controller's next untap step.")
        .FlavorText("Goblins, like snowflakes, are only dangerous in numbers.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever you cast a noncreature spell, you may pay {1}. If you do, tap target creature an opponent controls and it doesn't untap during its controller's next untap step.";
            p.Trigger(new OnCastedSpell((a, c) =>
              c.Controller == a.OwningCard.Controller && !c.Is().Creature));

            p.Effect = () => new PayManaThen(1.Colorless(), new CompoundEffect(
              new TapTargets(),
              new ApplyModifiersToTargets(() =>
                {
                  var modifier = new AddStaticAbility(Static.DoesNotUntap);

                  modifier.AddLifetime(new EndOfStep(
                    Step.Untap,
                    l => l.Modifier.SourceCard.Controller.IsActive));

                  return modifier;
                })));

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Creature, ControlledBy.Opponent).On.Battlefield(),
              trg => trg.Message = "Select a creature to tap.");

            p.TargetingRule(new EffectGiveDoesNotUntap());

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}