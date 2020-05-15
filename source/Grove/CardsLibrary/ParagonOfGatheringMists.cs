namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class ParagonOfGatheringMists : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Paragon of Gathering Mists")
        .ManaCost("{3}{U}")
        .Type("Creature — Human Wizard")
        .Text("Other blue creatures you control get +1/+1.{EOL}{U},{T}: Another target blue creature you control gains flying until end of turn.")
        .Power(2)
        .Toughness(2)
        .Cast(p =>
        {
          p.Effect = () => new CastPermanent().SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        })
        .ContinuousEffect(p =>
        {
          p.Modifier = () => new AddPowerAndToughness(1, 1);
          p.Selector = (c, ctx) => c.Controller == ctx.You && c.Is().Creature && c.HasColor(CardColor.Blue) && c != ctx.Source;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{U},{T}: Another target blue creature you control gains flying until end of turn.";
          p.Cost = new AggregateCost(
            new PayMana("{U}".Parse()), 
            new Tap());

          p.Effect = () => new ApplyModifiersToTargets(() => new AddSimpleAbility(Static.Flying) { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && c.HasColor(CardColor.Blue), controlledBy: ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield());

          p.TimingRule(new BeforeYouDeclareAttackers());
          p.TargetingRule(new EffectBigWithoutEvasions(c => !c.Has().Flying));
        });
    }
  }
}
