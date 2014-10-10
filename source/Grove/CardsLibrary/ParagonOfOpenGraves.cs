namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class ParagonOfOpenGraves : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Paragon of Open Graves")
        .ManaCost("{3}{B}")
        .Type("Creature — Skeleton Warrior")
        .Text("Other black creatures you control get +1/+1.{EOL}{2}{B},{T}: Another target black creature you control gains deathtouch until end of turn. {I}(Any amount of damage it deals to a creature is enough to destroy it.){/I}")
        .Power(2)
        .Toughness(2)
        .Cast(p =>
        {
          p.Effect = () => new PutIntoPlay().SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        })
        .ContinuousEffect(p =>
        {
          p.Modifier = () => new AddPowerAndToughness(1, 1);
          p.CardFilter = (c, e) => c.Controller == e.Source.Controller && c.Is().Creature && c.HasColor(CardColor.Black) && c != e.Source;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{2}{B},{T}: Another target black creature you control gains deathtouch until end of turn.";
          p.Cost = new AggregateCost(
            new PayMana("{2}{B}".Parse(), ManaUsage.Abilities),
            new Tap());

          p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Deathtouch) { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && c.HasColor(CardColor.Black), controlledBy: ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield());

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, controlledBy: ControlledBy.SpellOwner));
        });
    }
  }
}
