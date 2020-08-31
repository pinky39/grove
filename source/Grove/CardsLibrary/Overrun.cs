namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Overrun : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Overrun")
        .ManaCost("{2}{G}{G}{G}")
        .Type("Sorcery")
        .Text(
          "Creatures you control get + 3/+3 and gain trample until end of turn.")
        .FlavorText("Nature doesn't walk.")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
        {
          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,
            modifiers: L(
              () => new AddPowerAndToughness(3, 3) { UntilEot = true },
              () => new AddSimpleAbility(Static.Trample) { UntilEot = true }))
          .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TimingRule(new OnFirstMain());
        });
    }
  }
}