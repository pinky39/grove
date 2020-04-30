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

  public class KheruBloodsucker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kheru Bloodsucker")
        .ManaCost("{2}{B}")
        .Type("Creature — Vampire")
        .Text(
          "Whenever a creature you control with toughness 4 or greater dies, each opponent loses 2 life and you gain 2 life.{EOL}{2}{B}, Sacrifice another creature: Put a +1/+1 counter on Kheru Bloodsucker.")
        .FlavorText("It stares through the empty, pain-twisted faces of those it has drained.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a creature you control with toughness 4 or greater dies, each opponent loses 2 life and you gain 2 life.";
            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              selector: (c, ctx) => c.Controller == ctx.You && c.Is().Creature && c.Toughness >= 4));

            p.Effect = () => new CompoundEffect(
              new ChangeLife(2, P(e => e.Controller)),
              new ChangeLife(-2, P(e => e.Controller.Opponent)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}{B}, Sacrifice another creature: Put a +1/+1 counter on Kheru Bloodsucker.";

            p.Cost = new AggregateCost(
              new PayMana("{2}{B}".Parse()),
              new Sacrifice());

            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1))
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddCost(
              trg => trg.Is.Creature(ControlledBy.SpellOwner, canTargetSelf: false).On.Battlefield(),
              trg => trg.Message = "Select a creature to sacrifice.");

            p.TimingRule(new PumpOwningCardTimingRule(1, 1));
            p.TargetingRule(new EffectOrCostRankBy(c => c.Score) {TargetLimit = 1});
          });
    }
  }
}