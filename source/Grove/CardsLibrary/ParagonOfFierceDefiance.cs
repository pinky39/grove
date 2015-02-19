namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class ParagonOfFierceDefiance : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Paragon of Fierce Defiance")
        .ManaCost("{3}{R}")
        .Type("Creature — Human Warrior")
        .Text("Other red creatures you control get +1/+1.{EOL}{R},{T}: Another target red creature you control gains haste until end of turn. {I}(It can attack and {T} this turn.){/I}")
        .Power(2)
        .Toughness(2)
        .Cast(p =>
        {
          p.Effect = () => new CastPermanent().SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        })
        .ContinuousEffect(p =>
        {
          p.Modifier = () => new AddPowerAndToughness(1, 1);
          p.CardFilter = (c, e) => c.Controller == e.Source.Controller && c.Is().Creature && c.HasColor(CardColor.Red) && c != e.Source;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{R},{T}: Another target red creature you control gains haste until end of turn.";
          p.Cost = new AggregateCost(
            new PayMana("{R}".Parse()),
            new Tap());

          p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Haste) { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && c.HasColor(CardColor.Red), controlledBy: ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield());

          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, controlledBy: ControlledBy.SpellOwner));
        });
    }
  }
}
