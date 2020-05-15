namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class SanctifiedCharge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sanctified Charge")
        .ManaCost("{4}{W}")
        .Type("Instant")
        .Text(
          "Creatures you control get +2/+1 until end of turn. White creatures you control also gain first strike until end of turn.{I}(They deal combat damage before creatures without first strike.){/I}")
        .FlavorText("You need only raise your spear to receive this blessing.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ApplyModifiersToPermanents(
                selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,                
                modifier: () => new AddPowerAndToughness(2, 1) {UntilEot = true})
                .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness),
              new ApplyModifiersToPermanents(
                selector: (c, ctx) => c.Is().Creature && c.HasColor(CardColor.White) && ctx.You == c.Controller,                
                modifier: () => new AddSimpleAbility(Static.FirstStrike) {UntilEot = true}));

            p.TimingRule(new Any(
              new AfterOpponentDeclaresAttackers(),
              new AfterOpponentDeclaresBlockers()));
          });
    }
  }
}